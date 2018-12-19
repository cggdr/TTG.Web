
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Control.Models;

namespace TTG.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private AdministratorManager adminManager = new AdministratorManager();
        // GET: Control/Admin
        /// <summary>
        /// 添加【Json】
        /// </summary>
        /// <param name="addAdmin"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("AddPartialView")]
        public JsonResult AddJson(AddAdminViewModel addAdmin)
        {
            Response _res = new Response();
            if (ModelState.IsValid)
            {
                if (adminManager.HasAccounts(addAdmin.Accounts))
                {
                    _res.Code = 0;
                    _res.Message = "帐号已存在";
                }
                else
                {
                    Administrator _admin = new Administrator();
                    _admin.Accounts = addAdmin.Accounts;
                    _admin.CreateTime = System.DateTime.Now;
                    _admin.Password = Security.SHA256(addAdmin.Password);
                    _admin.LoginTime = DateTime.Now;

                    _res = adminManager.Add(_admin);
                }
            }
            else
            {
                _res.Code = 0;
                _res.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_res);
        }
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string _password = Security.SHA256(loginViewModel.Password);
                var _response = adminManager.Verify(loginViewModel.Accounts, _password);
                if (_response.Code == 1)
                {
                    var _admin = adminManager.Find(loginViewModel.Accounts);
                    Session.Add("AdminID", _admin.AdministratorID);
                    Session.Add("Accounts", _admin.Accounts);
                    _admin.LoginTime = DateTime.Now;
                    _admin.LoginIP = Request.UserHostAddress;
                    adminManager.Update(_admin);
                    return RedirectToAction("Index", "Admin");
                }
                else if (_response.Code == 2)
                {
                    ModelState.AddModelError("Accounts", _response.Message);

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
        public JsonResult ListJson()
        {
            return Json(adminManager.FindList(),JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddPartialView()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult DeleteJson(List<int> ids)
        {
            int _total = ids.Count();
            Response _res = new Response();
            int _currentAdminID = int.Parse(Session["AdminID"].ToString());
            if (ids.Contains(_currentAdminID))
            {
                ids.Remove(_currentAdminID);
            }
            _res = adminManager.Delete(ids);
            if (_res.Code == 1 && _res.Data < _total)
            {
                _res.Code = 2;
                _res.Message = "共提交删除" + _total + "名管理员。\n:原因:不能删除当前登录的账号";
            }
            else if (_res.Code == 2)
            {
                _res.Message = "共提交删除" + _total + "名管理员,实际删除" + _res.Data + "名管理员";
            }
            return Json(_res);
        }
        [HttpPost]
        public JsonResult ResetPassword(int id)
        {
            string _password = "123456";
            Response _res = adminManager.ChangePassword(id, Security.SHA256(_password));
            if (_res.Code == 1) _res.Message = "密码重置为：" + _password;
            return Json(_res);
        }
        public ActionResult MyInfo()
        {
            return View(adminManager.Find(Session["Accounts"].ToString()));
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyInfo(FormCollection form)
        {
            var _admin = adminManager.Find(Session["Accounts"].ToString());
            if (_admin.Password != form["Password"])
            {
                _admin.Password = Security.SHA256(form["Password"]);
                var _res = adminManager.ChangePassword(_admin.AdministratorID, _admin.Password);
                if (_res.Code == 1) ViewBag.Message = "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>修改密码成功！</div>";
                else ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>修改密码失败！</div>";
            }
            return View(_admin);
        }
    }
}