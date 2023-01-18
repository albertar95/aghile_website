using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SWProject.Models.EntityModel
{
    internal class Media_DataFilesMetadata
    {

        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "چیزی در باره این فایل بنویسید")]
        public string Caption { get; set; }

        [Display(Name = "مقدمه")]
        [DisplayName("مقدمه")]
        public string Introduction { get; set; }

        [Display(Name = "کلید واژه")]
        [DisplayName("کلید واژه")]
        public string Keywords { get; set; }

        [Display(Name = "توضیحات")]
        [DisplayName("توضیحات")]
        public string Description { get; set; }
    }
}

namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.Media_DataFilesMetadata))]
    partial class Media_DataFiles { }
}