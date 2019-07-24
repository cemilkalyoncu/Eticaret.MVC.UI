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
    [Table("Category")]
    public class Category:MyEntityBase
    {
        public Category()
        {
            Notes = new List<Note>();
        }

        [DisplayName("Kategori"), 
            Required(ErrorMessage ="{0} alanı gereklidir."), 
            StringLength(50,ErrorMessage ="{0} max. {1} karakter içermeli.")]
        public string Title { get; set;
        }
        [DisplayName("Açıklama"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(50, ErrorMessage = "{0} max. {1} karakter içermeli.")]
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; }

    }
}
