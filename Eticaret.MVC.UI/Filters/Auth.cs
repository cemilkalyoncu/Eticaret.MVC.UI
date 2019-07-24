using Eticaret.MVC.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.MVC.UI.Filters
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.UserSession == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}