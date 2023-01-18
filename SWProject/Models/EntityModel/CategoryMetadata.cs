using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SWProject.Models.EntityModel
{
    internal class CategoryMetadata
    {
        [Display(Name ="عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage ="لطفا عنوان دسته بندی را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "نمایه")]
        [DisplayName("نمایه")]
        public Nullable<System.Guid> ProfilePicId { get; set; }

        [Display(Name = "ریشه دارد؟")]
        [DisplayName("ریشه دارد؟")]
        [Required(ErrorMessage = "لطفا وضعیت ریشه دار بودن یا نبودن دسته بندی مشخص کنید")]
        public bool HasParent { get; set; }

        [Display(Name = "سر گروه")]
        [DisplayName("سر گروه")]
        public int HierarchyParent { get; set; }

        [Display(Name = "مقدمه")]
        [DisplayName("مقدمه")]
        public string Introduction { get; set; }

        [Display(Name = "فعال")]
        [DisplayName("فعال")]
        [Required(ErrorMessage = "لطفا وضعیت فعال بودن یا نبودن دسته بندی مشخص کنید")]
        public bool Enable { get; set; }

        [Display(Name ="توضیحات")]
        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [Display(Name = "کلید واژه")]
        [DisplayName("کلید واژه")]
        public string Keyword { get; set; }
    }
}

namespace SWProject.Models.DomainModel
{
    [MetadataType (typeof(SWProject.Models.EntityModel.CategoryMetadata))]
    partial class Category { }
}