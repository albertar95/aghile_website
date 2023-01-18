using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;


namespace SWProject.ViewModels.Admin
{
    public class PostDetailViewModel
    {
        public Post post { get; set; }
        public Paper_DataFiles paper { get; set; }
        public Media_DataFiles media { get; set; }
        public Media_DataFiles serat { get; set; }
    }
}