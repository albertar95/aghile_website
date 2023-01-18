using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SWProject.Models.EntityModel
{
    internal class LocalVariableMetadata
    {

        [Display(Name ="نوع")]
        [DisplayName("نوع")]
        [Required(ErrorMessage ="نوع متغییر را مشخص کنید")]
        public byte Type { get; set; }

        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "عنوان متغییر را مشخص کنید")]
        public string Title { get; set; }

        [Display(Name = "مقدار")]
        [DisplayName("مقدار")]
        [Required(ErrorMessage = "مقدار متغییر را مشخص کنید")]
        public string Value { get; set; }

        [Display(Name = "غیر قابل حذف")]
        [DisplayName("غیر قابل حذف")]
        [Required(ErrorMessage = "وضعیت غیر قابل حذف بودن یا  نبودن متغییر را مشخص نمایید")]
        public bool IsNecessary { get; set; }
    }
}
namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.LocalVariableMetadata))]
    partial class LocalVariable { }
}