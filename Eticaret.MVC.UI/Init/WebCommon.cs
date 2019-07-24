using Eticaret.Common;
using Eticaret.Entities;
using Eticaret.MVC.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eticaret.MVC.UI.Init
{
    public class WebCommon : ICommon
    {
        public string GetUserName()
        {
            TradeUser tradeUser = CurrentSession.UserSession;
            if (tradeUser != null)
                return tradeUser.UserName;
            else
                return "system";
        }
    }
}