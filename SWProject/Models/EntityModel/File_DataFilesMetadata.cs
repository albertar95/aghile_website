using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SWProject.Models.EntityModel
{
    internal class File_DataFilesMetadata
    {

        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "چیزی در باره این فایل بنویسید")]
        public string Caption { get; set; }

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
    [MetadataType(typeof(SWProject.Models.EntityModel.File_DataFilesMetadata))]
    partial class File_DataFiles { }
}