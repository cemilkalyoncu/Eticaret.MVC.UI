using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eticaret.MVC.UI.ViewModels
{
    public class WarringViewModel : NotifyViewModelBase<string>
    {
        public WarringViewModel()
        {
            Title = "Uyarı!";
        }
    }
}