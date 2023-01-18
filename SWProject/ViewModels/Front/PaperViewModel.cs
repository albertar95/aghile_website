using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel; 

namespace SWProject.ViewModels.Front
{
    public class PaperViewModel
    {
        public Paper_DataFiles paper { get; set; }
        public List<Paper_DataFiles> similar_paper { get; set; }

    }
}