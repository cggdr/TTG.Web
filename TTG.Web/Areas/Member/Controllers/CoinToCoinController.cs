using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Member.Models;

namespace TTG.Web.Areas.Member.Controllers
{
    [UserAuthorize]
    public class CoinToCoinController : Controller
    {
        private PriceTableManager _priceTableManager = new PriceTableManager();
        private EntrustManager _entrustManager = new EntrustManager();
        private EntrustDetailsManager _entrustDetails = new EntrustDetailsManager();
        private WalletManager _walletManager = new WalletManager();
        private UserManager _userManager = new UserManager();
        private EntrustDetailsManager _entrustsDetailsManager = new EntrustDetailsManager();
        private PriceInADealManager _priceInADealManager = new PriceInADealManager();
       
        private  static int ID;
        private static double Amount;
        private static int EntrusterID;
        private static string payCoin;
        private static string sellCoin;
        public ActionResult SaveID(int id,double amount,int entrusterID)
        {
            Amount = amount;
            ID = id;
            EntrusterID = entrusterID;
            return null;
        }
        public ActionResult SaveCoin(string _s)
        {
            int i = 0;
            int j = _s.Length - 1;
            while (i < _s.Length)
            {
                if (_s[i] != '/') i++;
                else break;
            }
          sellCoin = _s.Substring(0, i - 0);//出售的
          payCoin = _s.Substring(i + 1, j - i);//中转货币

            return null;
        }
        // GET: Member/CoinToCoin
        public ActionResult Index(string coin)
        {
            ViewBag.coin = coin;
            ViewBag.Success = 100;
            return View();
        }
        public JsonResult DayK(string cointocoin)
        {
            List<PriceTableViewModel> ptvm = new List<PriceTableViewModel>();
            if (cointocoin == null) cointocoin = "TTG/ETH";
            
            foreach(var  item in _priceTableManager.FindList(cointocoin))
            {
                ptvm.Add(new PriceTableViewModel
                {
                    KDateTime = item.KDateTime,
                    OpeningPrice = item.OpeningPrice,
                    ClosingPrice = item.ClosingPrice,
                    MaxPrice = item.MaxPrice,
                    MinPrice = item.MinPrice,
                    Amount = item.Amount

                });
           
            }
            return Json(ptvm);
        }
      public JsonResult AmountInDay(string cointocoin)
        {
            List<PriceInDayViewModel> _priceInDay = new List<PriceInDayViewModel>();
            if (cointocoin == null) cointocoin = "TTG/ETH";

            foreach (var item in _priceTableManager.FindListNowaday(cointocoin))
            {
                _priceInDay.Add(new PriceInDayViewModel
                {
                    KDateTime = item.KDateTime,
                    Price = item.ClosingPrice,
                    Amount = item.Amount

                });

            }
            return Json(_priceInDay);
        }

        public JsonResult Transaction(string coinname,int? id)
        {  if (id == null) id = -1;
            List<EntrustViewModel> _entrust = new List<EntrustViewModel>();
            foreach(var item in _entrustManager.FindListNowaday(coinname,id))
            {
                _entrust.Add(new EntrustViewModel
                {
                    SuccessTime = item.SucOrDefTime,
                    Price=item.Price,
                    Amount = item.Amount

                }
                );
            }
            return Json(_entrust);
        }
        public JsonResult ShowHall(string coinname)
        {
            List<HallViewModel> _hall = new List<HallViewModel>();
            foreach(var item in _entrustManager.FindListSell(coinname))
            {   //不显示自己的委托
                //    if (item.User.UserID == int.Parse(Session["UserID"].ToString())) continue;
                try
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                      
                        SelledAmount = _entrustDetails.FindRemainningAmount(item.ID),

                        RemainingAmount = item.Amount - _entrustDetails.FindRemainningAmount(item.ID)
                    });
                }
                catch//没产生交易 会产生空对象
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                      
                        SelledAmount = 0,

                        RemainingAmount = item.Amount
                    });
                }
            }
            return Json(_hall);
        }
        public ActionResult AddSalePartialView()
        {
            AddSale ad = new AddSale();
            ad.ID = ID;
           ad.PayCoin= _entrustManager.Find(ID).PayCoinName;
            ad.SellCoin= _entrustManager.Find(ID).CoinName;
            ad.Amount = 100;
            ad.Password = "";
            ad.price = _entrustManager.Find(ID).Price;
            return View(ad);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("AddSalePartialView")]
        public JsonResult AddJson(AddSale addSale)
        {
            Response _res = new Response();
            int id = int.Parse(Session["UserID"].ToString());
            double personCoinAmount = 0.00;
            try
            {
               personCoinAmount = _walletManager.GetWallet(id, addSale.PayCoin).Amount;
            }
            catch
            {

            }
            string _psd=null;
            if (Amount < addSale.Amount)
            {
                _res.Code = 0;
                _res.Message = "超过该委托寄售的货币数量";
                return Json(_res);
            }
           
            try {
                if (_userManager.Find(id).UserIdenty.Password == null)
                {

                }
            }
            catch
            {
                _res.Code = 0;
                _res.Message = "请先设置交易密码";
                return Json(_res);
            }
            try { _psd = Security.SHA256(addSale.Password); }
            catch {  _res.Code = 0;_res.Message = "请先输入交易密码"; return Json(_res); }
           
            if (_psd != _userManager.Find(id).UserIdenty.Password)
                {
                _res.Code = 0;
                _res.Message = "交易密码错误";
                return Json(_res);
                }
            if (personCoinAmount == 0.00 || (addSale.price * Amount) > personCoinAmount)
            {
                _res.Code = 0;
                _res.Message = "钱包余额不足，请前往充值";
                return Json(_res);
            }
            if (ModelState.IsValid)
            {  
              
            //减少支付货币 
                Wallet _wallet = _walletManager.GetWallet(id, addSale.PayCoin);
                _wallet.Amount -= addSale.Amount * addSale.price;
                _walletManager.Update(_wallet);
                //委托方增加货币
                Wallet _entrustWallet = _walletManager.GetWallet(EntrusterID, addSale.PayCoin);
                _entrustWallet.Amount+= addSale.Amount * addSale.price;
                _walletManager.Update(_entrustWallet);
                //更新委托 如果委托数量为零 则标记委托成功 记1，添加成功时间
                Entrust _entrust = new Entrust();
                _entrust = _entrustManager.Find(ID);
                if (Amount- addSale.Amount  == 0)
                { _entrust.IsSuccess = 1;
                    _entrust.SucOrDefTime = DateTime.Now;
                    _entrustManager.Update(_entrust);
                }
                else
                {  //不为零，添加委托详情表
                    EntrustDetails _entrustDetails =new EntrustDetails();
                    _entrustDetails.Amount = addSale.Amount;
                    _entrustDetails.BuyerID = id;
                    _entrustDetails.SellID = _entrustManager.Find(ID).FUserID;
                    _entrustsDetailsManager.Add(_entrustDetails);
                    //一笔交易成功 添加一日中每一笔的价格表
                    PriceInADeal _priceInADeal = new PriceInADeal();
                    _priceInADeal.DealTime = DateTime.Now;
                    _priceInADeal.Amount = addSale.Amount;
                    _priceInADeal.Price = addSale.price;
                    _priceInADealManager.Add(_priceInADeal);

                } //增加购买的货币
                Wallet _wallet2= _walletManager.GetWallet(id, addSale.SellCoin);
                _wallet2.Amount += addSale.Amount;
                _walletManager.Update(_wallet2);
                //委托方减少货币
                Wallet _entrustWallet2 = _walletManager.GetWallet(EntrusterID, addSale.SellCoin);
                _entrustWallet2.Amount -= addSale.Amount;
                _walletManager.Update(_entrustWallet2);

            }
            else
            {
                _res.Code = 0;
                _res.Message = General.GetModelErrorString(ModelState);
                return Json(_res);
            }
            _res.Code = 1;
            _res.Message = "购买成功";
            return Json(_res);
        }
        public ActionResult AddEntrust()//modelstate.isvalid=false  模型类与视图展示不匹配（属性的多少 名字）
        {
            ViewBag.Success =100;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEntrust(AddEntrust addEntrust)
        {
            Response _res = new Response();
            int id = int.Parse(Session["UserID"].ToString());
            if (string.IsNullOrEmpty(payCoin)) { payCoin = "TTG"; sellCoin = "ETH"; }
            addEntrust.EPayCoin = payCoin; 
            addEntrust.ESellCoin =sellCoin;
          
            double personCoinAmount = _walletManager.GetWallet(id, addEntrust.ESellCoin).Amount;

            if (personCoinAmount == 0 || (addEntrust.EAmount > personCoinAmount))
            {
                
                ViewBag.Success = "-1";
                ////return View("AddDefeat");

            }
         
            if (ModelState.IsValid)
            {
                Entrust entrust = new Entrust();
                entrust.CoinName = addEntrust.ESellCoin;
                entrust.PayCoinName = addEntrust.EPayCoin;
                entrust.IsSuccess = 0;
             
                entrust.FUserID= int.Parse(Session["UserID"].ToString());
                entrust.Price = addEntrust.Eprice;
                
                entrust.Amount = addEntrust.EAmount;
                entrust.EnstructTime = DateTime.Now;
                entrust.SucOrDefTime = DateTime.Now;
                _res=_entrustManager.Add(entrust);
                //减少币
                Wallet _wallet = _walletManager.GetWallet(id, addEntrust.ESellCoin);
                _wallet.Amount -= addEntrust.EAmount;
                _walletManager.Update(_wallet);
                ViewBag.Success = 1;

            }
            //if (!ModelState.IsValid)
            //{
            //    List<string> sb = new List<string>();
            //    //获取所有错误的Key
            //    List<string> Keys = ModelState.Keys.ToList();
            //    //获取每一个key对应的ModelStateDictionary
            //    foreach (var key in Keys)
            //    {
            //        var errors = ModelState[key].Errors.ToList();
            //        //将错误描述添加到sb中
            //        foreach (var error in errors)
            //        {
            //            sb.Add(error.ErrorMessage);
            //        }
            //    }
            //    return Json("0");
            //}
            return View("Index");
            //return Json(_res);

        }
        #region
        ///<summary>
        ///显示当前委托
        /// </summary>
        #endregion
        public JsonResult NowEn()
        {
            int id = int.Parse(Session["UserID"].ToString());
            string cointocoin = sellCoin + "/" + payCoin;
            List<HallViewModel> _hall = new List<HallViewModel>();
            foreach (var item in _entrustManager.FindListNowEn(id,cointocoin))
            {   //不显示自己的委托
                //    if (item.User.UserID == int.Parse(Session["UserID"].ToString())) continue;
                try
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        CoinToCoin = cointocoin,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                        SelledAmount = _entrustDetails.FindRemainningAmount(item.ID),
                        RemainingAmount = item.Amount - _entrustDetails.FindRemainningAmount(item.ID),
                        IsSuccess = item.IsSuccess
                    });
                }
                catch//没产生交易 会产生空对象
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        CoinToCoin = cointocoin,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                        SelledAmount = 0,
                        RemainingAmount = item.Amount,
                        IsSuccess = item.IsSuccess
                    });
                }
            }
            return Json(_hall);
       
        }
        #region
        ///<summary>
        ///显示当前委托
        /// </summary>
        #endregion
        public JsonResult HistoryEn()
        {
            int id = int.Parse(Session["UserID"].ToString());
            string cointocoin = sellCoin + "/" + payCoin;
            List<HallViewModel> _hall = new List<HallViewModel>();
            foreach (var item in _entrustManager.FindListHistoryEn(id, cointocoin))
            {   
                try
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                        CoinToCoin = cointocoin,
                        SelledAmount = _entrustDetails.FindRemainningAmount(item.ID),
                        RemainingAmount = item.Amount - _entrustDetails.FindRemainningAmount(item.ID),
                        IsSuccess = item.IsSuccess
                    });
                }
                catch//没产生交易 会产生空对象
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                        CoinToCoin = cointocoin,
                        SelledAmount = 0,
                        RemainingAmount = item.Amount,
                        IsSuccess = item.IsSuccess
                    });
                }
            }
            return Json(_hall);

        }


       
    }
}