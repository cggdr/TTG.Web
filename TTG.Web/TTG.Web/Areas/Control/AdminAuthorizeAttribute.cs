using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTG.Web.Areas.Control
{
    public class AdminAuthorizeAttribute:AuthorizeAttribute
    {
        //重写自定义授权检查
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["AdminID"] == null)
                return false;
            else return true;
        }
        ///<summary>
        ///重写为授权的http请求处理
        ///</summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Control/Admin/Login");
        }
    }
}