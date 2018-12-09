using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Control.Models;
using TTG.Web.Models;

namespace TTG.Web.Areas.Control.Controllers
{[AdminAuthorize]
    public class CoinController : Controller
    {
        private ApplicationManager _app = new ApplicationManager();
        private VirtualCurrencyManager _vir = new VirtualCurrencyManager();
        private PriceInDayManager _pr = new PriceInDayManager();
        private WalletManager _wallet = new WalletManager();
        private TypeManager _type = new TypeManager();
        public UserManager _user = new UserManager();
        private static string FileName;
        // GET: Control/Coin
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListJson()
        {
            return Json(_vir.FindList());
        }
        [HttpPost]
        public JsonResult DeleteJson(int id)
        {
            _wallet.Delete(id);
            return Json(_app.Delete(id));
        }
        public ActionResult ApplicationList()
        {
            return View();
        }
        public JsonResult AppListJson()
        {
            return Json(_app.FindList(),JsonRequestBehavior.AllowGet);
        }
        public FileStreamResult Download()
        {
            string fileName = FileName;//客户端保存的文件名
            string filePath = Server.MapPath("~/docs/"+ FileName);//路径
            return File(new FileStream(filePath, FileMode.Open), "text/plain",
            fileName);
        }
        [AllowAnonymous]
        public void SaveFileName(string fileName)
        {
            FileName = fileName;
        }
        public ActionResult Add()
        {
            var _types = new TypeManager().FindList();
            
            List<SelectListItem> _listItems = new List<SelectListItem>(_types.Count());
            foreach (var _type in _types)
            {
                _listItems.Add(new SelectListItem() { Text = _type.TypeName, Value = _type.ID.ToString() });//这里的属性名要与view中的model属性一致
            }
            ViewBag.Types = _listItems;
            return View();
        }
        private static bool IsEnglish(string en)
        {
            byte[] byte_len = System.Text.Encoding.Default.GetBytes(en);
            if (byte_len.Length == 2) { return false; }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddCTCViewModel ctc)
        {
            int id;
            if (!IsEnglish(ctc.Name)) { ModelState.AddModelError("Name", "请输入英文代称"); return View(ctc); }
            try
            {
                _vir.FindVirID(ctc.Name);
               
                ModelState.AddModelError("Name", "已存在该货币，且只可存在一种结算货币");
               
            }
            catch
            {
              
            }
            if (ModelState.IsValid)
            {
                Response _response = null;
                //添加的是结算货币 还需添加至货币体系
                if (ctc.ID == 5)
                {
                    Core.Type type = new Core.Type();
                    type.TypeName = ctc.Name;
                    _response = _type.Add(type);
                    VirtualCurrency vir = new VirtualCurrency();
                    vir.Name = ctc.Name;
                    vir.TypeID = ctc.ID;
                    vir.Description = ctc.Name + "/" + ctc.Name;
                    _response = _vir.Add(vir);
                    id = vir.VirCurID;
                   
                }//添加的是交易对 直接添加至货币体系
                else
                {
                    VirtualCurrency vir = new VirtualCurrency();
                    vir.Name = ctc.Name;
                    vir.TypeID = ctc.ID;
                    vir.Description = ctc.Name + "/" + _type.Find(ctc.ID).TypeName;
                    _response = _vir.Add(vir);
                    id = vir.VirCurID;
                }
                //都需要为每一个用户添加新的钱包
                List<int> users = _user.FindList().Select(u => u.UserID).ToList();
                    Wallet wallet = new Wallet();
                    foreach(var item in users)
                    {
                        wallet.Amount = 0;
                        wallet.UserID = item;
                        wallet.VirCurID = id;
                        _wallet.Add(wallet);
                    }
                //为新加的货币添加每日情况表
                PriceInDay pr = new PriceInDay {
                    CoinToCoin= ctc.Name + "/" + ctc.Name,
                    AmountInDay=0,
                    Price=0,
                    VolumeInDay=0,
                    MaxInDay=0,
                    MinInDay=0,
                    Up=0
            };
                _pr.Add(pr);

                if (_response.Code == 1) return View("Prompt", new Prompt()
                {
                    Title = "添加货币成功",
                    Message = "您已成功添加了货币【" + _response.Data.Name + "】",
                    Buttons = new List<string> {"<a href=\"" + Url.Action("Index", "Coin") + "\" class=\"btn btn-default\">交易对管理</a>",
                 "<a href=\"" + Url.Action("Add", "Coin") + "\" class=\"btn btn-default\">继续添加</a>"}
                });
                else ModelState.AddModelError("", _response.Message);

            }
            //这里还需初始化一次 如果不跳转页面
            var _types = new TypeManager().FindList();

            List<SelectListItem> _listItems = new List<SelectListItem>(_types.Count());
            foreach (var _type in _types)
            {
                _listItems.Add(new SelectListItem() { Text = _type.TypeName, Value = _type.ID.ToString() });//这里的属性名要与view中的model属性一致
            }
            ViewBag.Types = _listItems;
            return View(ctc);
        }
    }
}