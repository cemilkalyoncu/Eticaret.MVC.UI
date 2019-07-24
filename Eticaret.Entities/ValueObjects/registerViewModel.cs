using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eticaret.Entities.ValueObjects
{
    public class registerViewModel
    {
        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(150, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }
        [DisplayName("Parola"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password), 
            StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string Password { get; set; }
        [DisplayName("Parola"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password), 
            StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalıdır."),
            Compare("Password", ErrorMessage = "{0} ile {1} girilen parola uyuşmamaktadır.")]
        public string RePassword { get; set; }
        [DisplayName("Email"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(150, ErrorMessage = "{0} max. {1} karakter olmalıdır."),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }
    }
}