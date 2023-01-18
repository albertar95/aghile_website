using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SWProject.Models.EntityModel
{
    internal class UserMetadata
    {

        [Display(Name = "نام کاریری")]
        [DisplayName("نام کاریری")]
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "شماه همراه")]
        [DisplayName("شماره همراه")]
        [Required(ErrorMessage = "لطفا شماره همراه را وارد کنید")]
        public string Tel { get; set; }

        [Display(Name = "سطح دسترسی")]
        [DisplayName("سطح دسترسی")]
        [Required(ErrorMessage = "لطفا سطح دسترسی را تعیین کنید")]
        public byte Grade { get; set; }

        [Display(Name = "نمایه")]
        [DisplayName("نمایه")]
        public Nullable<System.Guid> ProfilePicId { get; set; }

        [Display(Name = "فعال")]
        [DisplayName("فعال")]
        [Required(ErrorMessage = "لطفا وضعیت فعال بودن یا نبودن کاربر را مشخص کنید")]
        public bool Enable { get; set; }
    }
}
namespace SWProject.Models.DomainModel
{
    [MetadataType(typeof(SWProject.Models.EntityModel.UserMetadata))]
    partial class User { }
}