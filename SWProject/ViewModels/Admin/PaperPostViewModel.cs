using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;

namespace SWProject.ViewModels.Admin
{
    public class PaperPostViewModel
    {
        public Post Post { get; set; }
        public Paper_DataFiles Paper { get; set; }
    }
}