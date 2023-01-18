using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;

namespace SWProject.ViewModels.Admin
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Category { get; set; }
    }
}