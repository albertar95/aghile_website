using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;


namespace SWProject.ViewModels.Front
{
    public class IndexViewModel
    {
        public IEnumerable<LocalVariable> slider { get; set; }

        public IEnumerable<Post> posts { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Post> TopCategoryPosts { get; set; }

        public IEnumerable<Post> lastseratposts { get; set; }

        public IEnumerable<Post> TopTypePosts { get; set; }
        public IEnumerable<Post> LastPosts { get; set; }

        public IEnumerable<LocalVariable> locals { get; set; }
        public IEnumerable<Post> PopularVideos { get; set; }

        public IEnumerable<File_DataFiles> LastBooks { get; set; }

        public IEnumerable<Post> LastPapers { get; set; }

        public IEnumerable<Doubt> QnD { get; set; }
    }
}