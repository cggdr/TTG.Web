using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Data;
using TTG.Web.Areas.Member.Models;

namespace TTG.Web.Areas.Member.Controllers
{
    [UserAuthorize]
    public class EntrustDocController : Controller
    {
        private EntrustManager _entrustManager = new EntrustManager();
        private EntrustDetailsManager _entrustDetails = new EntrustDetailsManager();
        private WalletManager _wallet = new WalletManager();

        // GET: Member/EntrustDoc
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult NowEn()
        {
            int id = int.Parse(Session["UserID"].ToString());
            string cointocoin ="";

            List<HallViewModel> _hall = new List<HallViewModel>();
            foreach (var item in _entrustManager.FindListNowEn(id, cointocoin))
            {   
                try
                {
                    _hall.Add(new HallViewModel
                    {
                        ID = item.ID,
                        EntrustTime = item.EnstructTime,
                        EntrusterID = item.FUserID,
                        CoinToCoin = item.CoinName + "/" + item.PayCoinName,
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
                        CoinToCoin = item.CoinName + "/" + item.PayCoinName,
                        Price = item.Price,
                        TotalAmount = item.Amount,
                        SelledAmount = 0,
                        RemainingAmount = item.Amount
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
            string cointocoin = "";
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
                        CoinToCoin = item.CoinName+"/"+item.PayCoinName,
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
                        CoinToCoin = item.CoinName + "/" + item.PayCoinName,
                        SelledAmount = 0,
                        RemainingAmount = item.Amount - _entrustDetails.FindRemainningAmount(item.ID),
                        IsSuccess = item.IsSuccess
                    });
                }
            }
            return Json(_hall);

        }
        public JsonResult SuccessfulEn()
        {
            int id = int.Parse(Session["UserID"].ToString());
            string cointocoin = "";
            List<HallViewModel> _hall = new List<HallViewModel>();
            foreach (var item in _entrustManager.FindListSuccessfulEn(id, cointocoin))
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
                        CoinToCoin = item.CoinName + "/" + item.PayCoinName,
                        SelledAmount = _entrustDetails.FindRemainningAmount(item.ID),
                        RemainingAmount = item.Amount - _entrustDetails.FindRemainningAmount(item.ID),
                        IsSuccess=item.IsSuccess
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
                        CoinToCoin = item.CoinName + "/" + item.PayCoinName,
                        SelledAmount = 0,
                        RemainingAmount = item.Amount,
                        IsSuccess = item.IsSuccess
                    });
                }
            }
            return Json(_hall);

        }
        public ActionResult CancelEntrust(int id,double amount,string coinname)
        {   //更新撤销后的委托情况
            Entrust en = _entrustManager.Find(id);
            en.IsSuccess = -1;
            _entrustManager.Update(en);
            //更新撤销后的钱包
            int i = 0;
            while (i < coinname.Length)
            {
                if (coinname[i] != '/') i++;
                else break;
            }
           coinname = coinname.Substring(0, i);//出售的
            Wallet wa = _wallet.GetWallet(int.Parse(Session["UserID"].ToString()), coinname);
            wa.Amount += amount;
            _wallet.Update(wa);
            return null;
        }
    }

}