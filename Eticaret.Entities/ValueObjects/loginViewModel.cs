using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eticaret.Entities.ValueObjects
{
    public class loginViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public string UserName { get; set; }
        [DisplayName("Parola"), Required(ErrorMessage = "{0} alanı boş geçilemez."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}