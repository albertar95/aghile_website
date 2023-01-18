using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SWProject.Models.EntityModel
{
    internal class DoubtMetadata
    {
        [Display(Name ="عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage ="لطفا عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "سوال")]
        [DisplayName("سوال")]
        [Required(ErrorMessage = "لطفا سوال را بنویسید")]
        public string Question { get; set; }

        [Display(Name = "پاسخ")]
        [DisplayName("پاسخ")]
        [Required(ErrorMessage = "لطفا پاسخ را وارد نمایید")]
        public string Answer { get; set; }

        [Display(Name = "فعال")]
        [DisplayName("فعال")]
        public bool Enable { get; set; }

        
    }
}
namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.DoubtMetadata))]
    partial class Doubt
    {

    }
}