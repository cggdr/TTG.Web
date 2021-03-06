﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTG.Core;
using TTG.Helper;
using TTG.Web.Areas.Control.Models;
using TTG.Web.Models;

namespace TTG.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();
        private VirtualCurrencyManager virManager = new VirtualCurrencyManager();
        private WalletManager walletManager = new WalletManager();
        private UserIdentyManager identyManager = new UserIdentyManager();
        [HttpPost]
        public JsonResult CanUserName(string UserName)
        {
            return Json(!userManager.HasUsername(UserName));
        }
        [HttpPost]
        public JsonResult CanEmail(string Email)
        {
            return Json(!userManager.HasEmail(Email));
        }
        /// <summary>
        /// 分页列表【json】
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="order">排序</param>
        /// <returns>Json</returns>
        public JsonResult ListJson(int? roleID, string username, string name, int? sex, string email, int? pageNumber, int? pageSize, int? order)
        {

            Paging<User> _pagingUser = new Paging<User>();
            if (pageNumber != null && pageNumber > 0) _pagingUser.PageIndex = (int)pageNumber;
            if (pageSize != null && pageSize > 0) _pagingUser.PageSize = (int)pageSize;
            var _paging = userManager.FindPageList(_pagingUser, roleID, username, name, sex, email, null);
            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
            //当序列化一张表时 该表与其他表相联可能会引起循环引用，需要db.Configuration.ProxyCreationEnabled = false; 
            //表和另一个表是有一对多关系的,当序列化表1的时候,会找到和另一个表2关联的字段,就会到另一个表2中序列化,然后另一个表2中也有一个字段和表1相关联.这样.序列化就会发生这种错误!
            //return Json(userManager.FindList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Control/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserViewModel userViewModel)
        {
            if (userManager.HasUsername(userViewModel.PhoneNumber)) ModelState.AddModelError("PhoneNumber", "用户名已存在");
            if (userManager.HasEmail(userViewModel.Email)) ModelState.AddModelError("Email", "Email已存在");
            if (ModelState.IsValid)
            {
                User _user = new User();
                _user.RoleID = userViewModel.RoleID;
                _user.PhoneNumber = userViewModel.PhoneNumber;
                _user.Name = userViewModel.Name;
                _user.Sex = userViewModel.Sex;
                _user.Password = Security.SHA256(userViewModel.Password);
                _user.Email = userViewModel.Email;
                _user.RegTime = DateTime.Now;
                _user.LastLoginTime = DateTime.Now;
                _user.LastLoginIP = Request.UserHostAddress;
                UserIdenty identy = new UserIdenty();
                identyManager.Add(identy);
                _user.FKIdentyID = identy.ID;
                var _response = userManager.Add(_user);
                
               
                Wallet wallet = new Wallet();   
                foreach (var item in virManager.FindAllVirID())
                {
                    wallet.UserID = _user.UserID;
                    wallet.VirCurID = item;
                    wallet.Amount = 0;
                    walletManager.Add(wallet);
                   
                }  
                if (_response.Code == 1) return View("Prompt", new Prompt()
                {
                    Title = "添加用户成功",
                    Message = "您已成功添加了用户【" + _response.Data.PhoneNumber + "（" + _response.Data.Name + "）】",
                    Buttons = new List<string> {"<a href=\"" + Url.Action("Index", "User") + "\" class=\"btn btn-default\">用户管理</a>",  
                 "<a href=\"" + Url.Action("Add", "User") + "\" class=\"btn btn-default\">继续添加</a>"}
                });
                else ModelState.AddModelError("", _response.Message);
            }
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束

            return View(userViewModel);
        }
      
        
    }
}