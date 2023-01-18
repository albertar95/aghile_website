using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SWProject.Models.EntityModel
{
    internal class PostMetadata
    {
        [Display(Name ="عنوان پست")]
        [DisplayName("عنوان پست")]
        [Required(ErrorMessage ="لطفا عنوان پست را مشخص نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع پست")]
        [DisplayName("نوع پست")]
        public byte Type { get; set; }

        [Display(Name = "دسته بندی")]
        [DisplayName("دسته بندی")]
        [Required(ErrorMessage = "لطفا دسته بندی را مشخص نمایید")]
        public int CategoryId { get; set; }

        [Display(Name = "نمایه پست")]
        [DisplayName("نمایه پست")]
        public Nullable<System.Guid> ProfilePicId { get; set; }

        [Display(Name = "فعال")]
        [DisplayName("فعال")]
        [Required(ErrorMessage = "لطفا وضعیت فعال بودن یا نبودن پست را مشخص نمایید")]
        public bool Enable { get; set; }

        [Display(Name = "نمایش در اسلایدر")]
        [DisplayName("نمایش در اسلایدر")]
        public bool IsIndex { get; set; }
    }
}

namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.PostMetadata))]
    partial class Post { }
}

