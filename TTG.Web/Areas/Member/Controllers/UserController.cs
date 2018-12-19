using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Member.Models;


namespace TTG.Web.Areas.Member.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();
        private UserIdentyManager identyManager = new UserIdentyManager();
        private PriceInDayManager priceInDayManager = new PriceInDayManager();
        private VirtualCurrencyManager virManager = new VirtualCurrencyManager();
        private WalletManager walletManager = new WalletManager();
        private ApplicationManager applicationManager = new ApplicationManager();
        
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CanUserName(string UserName)
        {
            return Json(!userManager.HasUsername(UserName));
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ExistPhoneNumber(string UserName)
        {
            return Json(!userManager.HasUsername(UserName));
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CanEmail(string Email)
        {
            return Json(!userManager.HasEmail(Email));
        }
        // GET: Member/User
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.coin = "TTG/ETH";   
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string _password = Security.SHA256(loginViewModel.Password);
                var _response = userManager.Verify(loginViewModel.PhoneNumber, _password);
                ViewBag.code = _response.Code;
                if (_response.Code == 1)
                {
                    var _user = userManager.Find(loginViewModel.PhoneNumber);
                    Session.Add("UserID", _user.UserID);
                    Session.Add("UserName", _user.PhoneNumber);
                    _user.LastLoginTime = DateTime.Now;
                    _user.LastLoginIP = Request.UserHostAddress;
                    userManager.Update(_user);
                    return RedirectToAction("Index", "User");
                }
                else if (_response.Code == 2)
                {
                    ModelState.AddModelError("PhoneNumber", _response.Message);

                }
                else if (_response.Code == 3)
                    ModelState.AddModelError("Password", _response.Message);
                else ModelState.AddModelError("", _response.Message);
             
            }
            return View(loginViewModel);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
     
        //[AllowAnonymous]
        //时刻谨记为控制器加了授权后，其所有方法都需授权才可访问
        //public ActionResult VerificationCode()
        //{    //图片验证
        //    //string verificationCode = Security.CreateVerificationText(6);
        //    //ViewBag.code = verificationCode;
        //    //Bitmap _img = Security.CreateVerificationImage(verificationCode, 160, 30);
        //    //_img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    //TempData["VerificationCode"] = verificationCode.ToUpper();
        //    //短信验证
        //    string verificationCode = Security.CreateVerificationText(6);
        //    TempData["VerificationCode"] = verificationCode;
        //    return null;
        //}
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Success = 100;   
            return View();
        }
        //[AllowAnonymous]
        //public static void HttpPost(string Url, string postDataStr)
        //{
        //    byte[] dataArray = Encoding.UTF8.GetBytes(postDataStr);
        //    // Console.Write(Encoding.UTF8.GetString(dataArray));
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = dataArray.Length;
        //    //request.CookieContainer = cookie;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(dataArray, 0, dataArray.Length);
        //    dataStream.Close();
        //    try
        //    {
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //        String res = reader.ReadToEnd();
        //        reader.Close();
        //        //Console.Write("\nResponse Content:\n" + res + "\n");
        //    }
        //    catch (Exception e)
        //    {
        //        //Console.Write(e.Message + e.ToString());
        //    }
        //}
        [AllowAnonymous]
        public ActionResult GetCheckNum(string phoneNum)
        {
          
            //账号
            string account = "15170028673";
            //密码
            string pswd = "cg..123123";
            //修改为您要发送的手机号
            string mobile = phoneNum;
            // 短信发送接口的http地址，请咨询客服
            string url = "http://139.196.108.241:8080/Api/HttpSendSMYzm.ashx";

            // 发验短信调用示例
            // 发送内容 
            string code = Security.CreateVerificationText(6);
            TempData["VerificationCode"] = code ;
            string msg = "您的验证码为" + code;
            string data1 = "account=" + account + "&pswd=" + pswd + "&mobile=" + mobile + "&msg=" + msg + "&needstatus=true";
            Sms.HttpPost(url, data1);
            return Json(new { msg = true,data1});
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            ViewBag.Success = -1;
            if (userManager.HasUsername(registerViewModel.PhoneNumber)) ModelState.AddModelError("PhoneNumber", "注册手机号已存在");
            if (userManager.HasEmail(registerViewModel.Email)) ModelState.AddModelError("Email", "Email已存在");
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != registerViewModel.VerificationCode)
            {
                ModelState.AddModelError("VerificationCode", "验证码不正确");
                return View(registerViewModel);
            }
            if (ModelState.IsValid)
            {
                User _user = new User();
                _user.RoleID = 1;
                _user.PhoneNumber = registerViewModel.PhoneNumber;
                _user.Name = registerViewModel.Name;
                _user.Sex = registerViewModel.Sex;
                _user.Password = Security.SHA256(registerViewModel.Password);
                _user.Email = registerViewModel.Email;
                _user.RegTime = DateTime.Now;
                UserIdenty identy = new UserIdenty();
                identyManager.Add(identy);
                _user.FKIdentyID = identy.ID;
                var _response = userManager.Add(_user);
                try {               
                    Session.Add("UserID", _user.UserID);
                    Session.Add("UserName", _user.PhoneNumber);
                    _user.LastLoginTime = DateTime.Now;
                    _user.LastLoginIP = Request.UserHostAddress;
                    userManager.Update(_user);
                    Wallet wallet = new Wallet();
                   foreach(var item in virManager.FindAllVirID())
                    {
                        wallet.UserID = int.Parse(Session["UserID"].ToString());
                        wallet.VirCurID = item;
                        wallet.Amount = 0;
                       Response res= walletManager.Add(wallet);

                    }
                    return RedirectToAction("Index", "User");
             
                     }
                catch
                { 
                ModelState.AddModelError("", _response.Message);
                }
               
                ViewBag.Success = 1;
            }
            return View(registerViewModel);
        }
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgetPassword(ForgetPasswordViewModel _forgetPwd)
        {
            if (!userManager.HasUsername(_forgetPwd.PhoneNumber)) ModelState.AddModelError("PhoneNumber", "该手机号未注册");
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != _forgetPwd.VerificationCode)
            {
                ModelState.AddModelError("VerificationCode", "验证码不正确");
                return View(_forgetPwd);
            }
            if (ModelState.IsValid)
            {
                var _password = Security.SHA256(_forgetPwd.Password);
                User _forgetUser =userManager.Find(_forgetPwd.PhoneNumber);
                Response _res =userManager.ChangePassword(_forgetUser.UserID, _password);
                if (_res.Code == 1) { return RedirectToAction("Login", "User"); }
                else ModelState.AddModelError("PhoneNumber", "未知错误请联系客服人员");
            }
            return View(_forgetPwd);

        }
        [AllowAnonymous]
        public ActionResult RegProtocol()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ServiceProtocol()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult PrivacyProtocol()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Rate()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AboutWe()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult JoinWe()
        {
            return View();
        }
        //[AllowAnonymous]
        //public ActionResult Application()
        // {

        //     return View();
        // }
        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public ActionResult Application(ApplicationViewModel ap)
        // {

        //     if(ModelState.IsValid)
        //     { 
        //     Application app = new Core.Application();
        //     app.ApplicantName = ap.ApplicantName;
        //     app.CoinName = ap.CoinName;
        //     app.CompanyName = ap.CompanyName;
        //     app.CreateTime = DateTime.Now;
        //     app.Email = ap.Email;
        //     app.PhoneNum = ap.PhoneNum;
        //     app.ReviewTime = DateTime.Now;
        //     app.Status = 0;

        //     Response _res=applicationManager.Add(app);
        //     if(_res.Code==1)
        //     ViewBag.Success = 1;
        //     }
        //     return View(ap);
        // }

        [AllowAnonymous]
        public ActionResult Application()
        {
            ViewBag.Success = -100;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Application(ApplicationViewModel ap, HttpPostedFileBase MyFile)
        {
            if (MyFile == null)
            {
                ViewBag.Success = 0;
                return View(ap);
            }
            if (MyFile.ContentLength > 4194304)
            {
                ViewBag.Success = 100;
                return View(ap);
            }
            //得到的名字是文件在本地机器的绝对路径
            var strLocalFullPathName = MyFile.FileName;
            //提取出单独的文件名，不需要路径
            var strFileName = Path.GetFileName(ap.CoinName + " -" + strLocalFullPathName);
            //定义服务器的文件夹，用来保存文件
            var strServerFilePath = Server.MapPath("~/docs/");
            //将接收到文件保存在服务器指定上
            MyFile.SaveAs(Path.Combine(strServerFilePath, strFileName));
            if (ModelState.IsValid)
            {
                Application app = new Core.Application();
                app.ApplicantName = ap.ApplicantName;
                app.CoinName = ap.CoinName;
                app.CompanyName = ap.CompanyName;
                app.CreateTime = DateTime.Now;
                app.Email = ap.Email;
                app.PhoneNum = ap.PhoneNum;
                app.ReviewTime = DateTime.Now;
                app.Status = 0;
                app.FileAddress =strFileName;
                Response _res = applicationManager.Add(app);
                if (_res.Code == 1)
                    ViewBag.Success = 1;
            }


            return View(ap);
        }
        [AllowAnonymous]
        public JsonResult ListJson(string s)
        {
            //List<PriceInDayView> a= priceInDayManager.FindList(s);
            // a[i].Coinname a[i].PayCoinName a[i].Amount 再新建视图模型（3个相同属性） list赋值
            ViewBag.coin ="TTG/ETH";
            ViewBag.cointocoin = null;
            return Json(priceInDayManager.FindList(s),JsonRequestBehavior.AllowGet);
        }
       [AllowAnonymous]
       public ActionResult CoinDetails(string coin)
        {
            return View();
        }
    

    }
}