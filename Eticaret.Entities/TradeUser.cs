using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Entities
{
    [Table("TradeUser")]
    public class TradeUser : MyEntityBase
    {
        [DisplayName("Adı"), 
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }
        [DisplayName("Soyadı"), 
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }
        [DisplayName("E-Posta"), 
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFileName { get; set; }

        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likeds { get; set; }
    }
}
