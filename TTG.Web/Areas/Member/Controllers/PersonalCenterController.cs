using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Member.Models;

namespace TTG.Web.Areas.Member.Controllers
{
    [UserAuthorize]
    public class PersonalCenterController : Controller
    {
        private UserManager _user = new UserManager();
        private WalletManager _wallet = new WalletManager();
        private VirtualCurrencyManager _vir = new VirtualCurrencyManager();
        private InteractionManager _interaction = new InteractionManager();
        private UserIdentyManager _indenty = new UserIdentyManager();
        private static int WalletID;
        private static double Amount;
        public ActionResult SaveWalletID(int id,double amount)
        {
            WalletID = id;
            Amount = amount;
            return null;
        }

        // GET: Member/PersonalCenter
        public ActionResult Index()
        {
            return View("Assets");
        }
        public ActionResult Assets()
        {
            return View();
        }
        public JsonResult AssetsJson()
        {
            int id = int.Parse(Session["UserID"].ToString());
            List<AssetsViewModel> assets= new List<AssetsViewModel>();
            List<Wallet> wa = _wallet.FindList(u => u.UserID == id).ToList();
            foreach (var item in wa)
            {
                assets.Add(new AssetsViewModel
                {  WalletID=item.WalletID,
                    CoinName =_wallet.GetWalletName(item.VirCurID),
                    Amount=item.Amount
                });
                
            };
            return Json(assets);
        }
        public ActionResult AddInPartialView()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddInPartialView(AddInViewModel addIn)
        {
            Response _res = new Helper.Response();
            if (ModelState.IsValid)
            {
                Interaction interaction = new Interaction();
                interaction.FWalletID = WalletID;
                interaction.Amount = addIn.Amount;
                interaction.WalletAddress = addIn.WalletAddrress;
                interaction.Status = 0;
                interaction.InOrOut = 1;
                interaction.OperatingTime = DateTime.Now;
                 _res= _interaction.Add(interaction);
                
            }
            if (_res.Code == 1) _res.Message = "已提交申请";
            return Json(_res);
        }


        public ActionResult AddOutPartialView()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOutPartialView(AddInViewModel addIn)
        {
            Response _res = new Helper.Response();
            if (Amount < addIn.Amount) { _res.Code = 0;_res.Message = "提现数量大于您当前持有的货币额"; return Json(_res); }
            if (ModelState.IsValid)
            {
                Interaction interaction = new Interaction();
                interaction.FWalletID = WalletID;
                interaction.Amount = addIn.Amount;
                interaction.WalletAddress = addIn.WalletAddrress;
                interaction.Status = 0;
                interaction.InOrOut = -1;
                interaction.OperatingTime = DateTime.Now;
                _res = _interaction.Add(interaction);

            }
            if (_res.Code == 1) _res.Message = "已提交申请";
            return Json(_res);
        }
        public ActionResult Bill()
        { 
            return View();
        }
        public JsonResult Interaction(int id)
        {
            int ID= int.Parse(Session["UserID"].ToString());
            List<BillViewModel> bv = new List<BillViewModel>();
            List<Interaction> inter = _interaction.GetBill(ID).FindAll(u => u.InOrOut == id);
            int virid;
            foreach (var item in inter)
            {
                virid = _wallet.Find(item.FWalletID).VirCurID;//lamada不要写的太复杂，最好条件不要嵌套使用，很可能会识别不出来，直接报错！！！
                //try
                //{
                    bv.Add(new BillViewModel
                    {
                        OperatingTime = item.OperatingTime,
                        CoinName = _vir.Find(u=>u.VirCurID==virid).Name,
                        WalletAddress = item.WalletAddress,
                        Amount = item.Amount,
                        Status = item.Status,
                    });
                //}
                //catch
                //{

                //}
            }
            return Json(bv);
        }
       public ActionResult ChangePW()
        {
            ViewBag.Message = "<div class=\"alert alert-warning\" role=\"alert\"><span class=\"glyphicon glyphicon-star\"></span>修改登录密码</div>";
            return View();
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePw(ChangePWViewModel cpw)
        {  
            User user =_user.Find(int.Parse(Session["UserID"].ToString()));
            if (Security.SHA256(cpw.Password) != user.Password) { ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>与原来的密码不一致</div>"; return View(cpw); }
            if (ModelState.IsValid)
            {
               
                Response _res = new Helper.Response();
                if (Security.SHA256(cpw.ConfirmPassword) != user.Password)
                {
                    _res = _user.ChangePassword(user.UserID, Security.SHA256(cpw.ConfirmPassword));
                    if (_res.Code == 1) { ViewBag.Message = "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>修改密码成功！</div>";   }
                    else ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>修改密码失败！</div>";
                }
            }
            return View(cpw);
        }
        public ActionResult ChangeTransactionPw()
        {
            User user = _user.Find(int.Parse(Session["UserID"].ToString()));
            
            if (string.IsNullOrEmpty(_indenty.Find(user.FKIdentyID).Password)) ViewBag.Message = "<div class=\"alert alert-warninig\" role=\"alert\"><span class=\"glyphicon glyphicon-star-empty\"></span>设置交易密码(两次密码一致)</div>";
            else ViewBag.Message = "<div class=\"alert alert-warning\" role=\"alert\"><span class=\"glyphicon glyphicon-star\"></span>修改交易密码</div>";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTransactionPw(ChangePWViewModel cpw)
        {
            User user = _user.Find(int.Parse(Session["UserID"].ToString()));
            Response _res = new Helper.Response();
            if (string.IsNullOrEmpty(_indenty.Find(user.FKIdentyID).Password))
            {
                if (ModelState.IsValid)
                {
                    _res = _user.ChangeTransactionPassword(user.UserID, Security.SHA256(cpw.ConfirmPassword));
                    if (_res.Code == 1) { ViewBag.Message = "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>设置交易密码成功！</div>"; }
                    else ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>设置交易密码失败</div>";
                }
            }
            else
            {
                if (Security.SHA256(cpw.Password) != _indenty.Find(user.FKIdentyID).Password) { ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>与原来的密码不一致</div>"; return View(cpw); }
                if (ModelState.IsValid)
                {
                        _res = _user.ChangeTransactionPassword(user.UserID, Security.SHA256(cpw.ConfirmPassword));
                        if (_res.Code == 1) { ViewBag.Message = "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>修改密码成功！</div>"; }
                        else ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>修改密码失败！</div>";
                }
            }

            return View(cpw);
        }
    }
}