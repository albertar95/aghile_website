using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;

namespace SWProject.ViewModels.Front
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Post> medias { get; set; }

        public IEnumerable<Post> serats { get; set; }

        public IEnumerable<Post> papers { get; set; }
        
        public IEnumerable<File_DataFiles> files { get; set; }

        public string ThisCategory { get; set; }
    }
}