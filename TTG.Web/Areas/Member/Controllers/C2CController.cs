using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Member.Models;

namespace TTG.Web.Areas.Member.Controllers
{
    public class C2CController : Controller
    {
        private WalletManager _wallet = new WalletManager();
        private EntrustManager _entrust = new EntrustManager();
        private UserManager _userManager=new UserManager();
        // GET: Member/C2C
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(C2CViewModel c2c)

        {
            
            string _psd=null;
            int id = int.Parse(Session["UserID"].ToString());
            string traPsd = _userManager.Find(id).UserIdenty.Password;
            ViewBag.Success = 0;
            if (c2c.Amount > _wallet.GetWallet(id, "CNYX").Amount)
            { ModelState.AddModelError("Amount", "持有货币量不足");return View(); }
            try { _psd = Security.SHA256(c2c.C2CPassword); }
            catch {  ModelState.AddModelError("C2CPassword", "请先输入交易密码"); }
            try
            {
                if (traPsd == null) { }
            }
            catch
            {
                ModelState.AddModelError("C2CPassword", "请先设置交易密码");
                return View();
            }
            if (_psd != traPsd)
            {
                ModelState.AddModelError("C2CPassword", "交易密码错误");
                return View();
            }
            if (ModelState.IsValid)
            {
                Entrust en = new Entrust();
                en.Amount = c2c.Amount;
                en.CoinName = "CNYX";
                en.EnstructTime = DateTime.Now;
                en.FUserID = id;
                en.IsSuccess = 0;
                en.PayCoinName = "CNY";
                en.Price = 0.99;
                en.SucOrDefTime = DateTime.Now;
                _entrust.Add(en);
                ViewBag.Success = 1;
            }
          
            return View();
        }
    }
}