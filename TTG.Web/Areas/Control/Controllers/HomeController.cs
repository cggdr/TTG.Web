using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TTG.Core;
using System.Data.Entity;
using TTG.Web.Areas.Control.Models;

namespace TTG.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private UserManager _userManager = new UserManager();
        private PriceInDayManager _priceInDaymanager = new PriceInDayManager();
        private PriceInADealManager _priceInADealManager = new PriceInADealManager();
        // GET: Control/Home
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Sex()
        {
            List<SexPie> sexPie = new List<SexPie>();
            sexPie.Add(new SexPie
            {
                value = _userManager.FindList(u => u.Sex == 1).Count(),
                name = "男"
            });
            sexPie.Add(new SexPie
            {
                value = _userManager.FindList(u => u.Sex == 0).Count(),
                name = "女"
            });
            sexPie.Add(new SexPie
            {
                value = _userManager.FindList(u => u.Sex == -1).Count(),
                name = "未知"
            });

            return Json(sexPie);
        }
        public JsonResult Coin()
        {
            List<CoinBar> coinBar = new List<CoinBar>();
            List<PriceInDay> pd = _priceInDaymanager.FindList().ToList();
            foreach (var item in pd) {
                coinBar.Add(new CoinBar {
                    coin = item.CoinToCoin ,
                    volumn=item.VolumeInDay 
                });
            }
            return Json(coinBar);
        }
        public JsonResult Entrust()
        {   //按ID分组 查找出所有交易对各自的数量 交易笔
            var pd= _priceInADealManager.FindList(p => DbFunctions.DiffHours(p.DealTime,DateTime.Now) < 24).GroupBy(u=>u.PriceInDayID).ToList();
           string  name = null;
            List<EntrustDataSet> eds = new List<EntrustDataSet>();
            List<string> edsCoinName = new List<string>();
            List<string> cointocoin = new List<string>();
            //找出所有交易对名称
             var list=_priceInDaymanager.FindList().ToList().Select(u => u.CoinToCoin);
            foreach(var item in list)
            {
                cointocoin.Add(item);
            };
            //将分组情况添加到json类中
            foreach(var i in pd)
            {
                foreach(var item in i) {
                    name= _priceInDaymanager.Find(item.PriceInDayID).CoinToCoin;
                    break;
                }
                eds.Add(new EntrustDataSet{
                    
                    count=i.Count(),
                    amount =i.Sum(u => u.Amount),
                    coinname =name
                   
                });
            };
            foreach (var item in eds)
            {
                edsCoinName.Add(item.coinname);
            }
            //如果某交易对无交易 添加默认
            foreach(var item in cointocoin)
            {
                if (edsCoinName==null||!edsCoinName.Contains(item))
                {
                    eds.Add(new EntrustDataSet
                    {

                        count = 0,
                        amount = 0,
                        coinname = item

                    });
                };
            }

            return Json(eds);
        }
        public JsonResult UserLogin()
        {
            List<UserLoginLine> userLogin = new List<UserLoginLine>();
            for (int i = 0; i < 7; i++)
            {

                userLogin.Add(new UserLoginLine
                {
                    date = DateTime.Now.Date.AddDays(-1 * i),
                    amount = _userManager.LoginUser(i)
                    }
                     );
                
            }


            return Json(userLogin);
        }
    }
}