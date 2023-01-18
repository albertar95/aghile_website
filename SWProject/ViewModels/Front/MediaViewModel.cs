using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;

namespace SWProject.ViewModels.Front
{
    public class MediaViewModel
    {
        public Media_DataFiles media { get; set; }

        public Media_DataFiles serat { get; set; }

        public List<Media_DataFiles> similars { get; set; }
    }
}