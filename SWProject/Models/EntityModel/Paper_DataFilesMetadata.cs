using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SWProject.Models.EntityModel
{
    internal class Paper_DataFilesMetadata
    {
        [Display(Name ="عنوان مقاله")]
        [DisplayName("عنوان مقاله")]
        [Required(ErrorMessage = "لطفا عنوان مقاله را مشخص نمایید")]
        public string Title { get; set; }

        [Display(Name = "چکیده")]
        [DisplayName("چکیده")]
        public string Summary { get; set; }

        [Display(Name = "مقدمه")]
        [DisplayName("مقدمه")]
        public string Introduction { get; set; }

        [Display(Name = "متن")]
        [DisplayName("متن")]
        [Required(ErrorMessage = "مقاله فاقد متن می باشد")]
        public string Body { get; set; }

        [Display(Name = "نویسندگان")]
        [DisplayName("نویسندگان")]
        [Required(ErrorMessage = "لطفا نویسندگان مقاله را مشخص نمایید")]
        public string Authors { get; set; }

        [Display(Name = "نتیجه گیری")]
        [DisplayName("نتیجه گیری")]
        public string Conclution { get; set; }

        [Display(Name = "کلید واژه")]
        [DisplayName("کلید واژه")]
        public string Keywords { get; set; }

        [Display(Name = "توضیحات")]
        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [Display(Name = "منابع")]
        [DisplayName("منابع")]
        public string Source { get; set; }
    }
}

namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.Paper_DataFilesMetadata))]
partial class Paper_DataFiles { }
}