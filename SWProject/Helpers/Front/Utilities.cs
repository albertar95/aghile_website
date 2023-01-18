using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;
using System.Collections;

namespace SWProject.Helpers.Front
{
    public class Utilities
    {
        static SwpEntities db = null;
        static ArrayList Result = null;

        public static string getInsta()
        {
            try
            {
                db = new SwpEntities();
                return db.LocalVariables.Where(p => p.Type == 0 && p.Title == "instagram").ToList().First().Value;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<LocalVariable> IndexSlider()
        {
            try
            {
                db = new SwpEntities();
                return db.LocalVariables.Where(p => p.Type == 3).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }
        public static IEnumerable<Post> IndexPosts()
        {
            try
            {
                db = new SwpEntities();
                return db.Posts.Where(p => p.IsIndex == true && p.Enable == true).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> LastPosts()
        {
            db = new SwpEntities();
            List<Post> result = new List<Post>();
            try
            {
                //foreach (var item in db.Posts.OrderByDescending(p => p.CreationDate).Take(3))
                //{
                //    result.Add(item);
                //}
                foreach (var item in db.Posts.Where(p => p.Type == 2 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(8))
                {
                    result.Add(item);
                }
                foreach (var item in db.Posts.Where(p => p.Type == 3 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(8))
                {
                    result.Add(item);
                }
                foreach (var item in db.Posts.Where(p => p.Type == 1 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(8))
                {
                    result.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static IEnumerable<Post> LastseratPosts()
        {
            db = new SwpEntities();
            List<Post> result = new List<Post>();
            try
            {
                foreach (var item in db.Posts.Where(p => p.Type == 4 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(4))
                {
                    result.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static IEnumerable<Category> Categories()
        {
            try
            {
                db = new SwpEntities();
                return db.Categories.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> TopCategoryPost()
        {
            db = new SwpEntities();
            List<Post> result = new List<Post>();
            try
            {
                var topcat = db.Categories.Where(q => q.Priority != null).OrderBy(p => p.Priority).First().HierarchyId;
                //return db.Posts.Where(p => p.CategoryId == topcat).Take(2).ToList();
                foreach(var item in db.Posts.Where(p => p.CategoryId == topcat).Take(2))
                {
                    result.Add(item);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public static IEnumerable<Post> TopTypePost()
        {
            db = new SwpEntities();
            List<Post> result = new List<Post>();
            try
            {
                var vids = db.Posts.Where(p => p.Type == 2 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(4);
                var vois = db.Posts.Where(p => p.Type == 1 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(4);
                foreach (var item in vids)
                {
                    result.Add(item);
                }
                foreach (var item in vois)
                {
                    result.Add(item);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public static IEnumerable<Post> LastPapers()
        {
            db = new SwpEntities();
            try
            {
                    return db.Posts.Where(p => p.Type == 3 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<File_DataFiles> LastFiles()
        {
            db = new SwpEntities();
            try
            {
                    return db.File_DataFiles.OrderByDescending(p => p.TimeStamp).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Doubt> Qnds()
        {
            db = new SwpEntities();
            List<Doubt> result = new List<Doubt>();
            try
            {
                //foreach (var item in db.Posts.OrderByDescending(p => p.CreationDate).Take(3))
                //{
                //    result.Add(item);
                //}
                foreach (var item in db.Doubts.Where(p => p.Type == false && p.Enable == true).OrderByDescending(q => q.TimeStamp).Take(10))
                {
                    result.Add(item);
                }
                foreach (var item in db.Doubts.Where(p => p.Type == true && p.Enable == true).OrderByDescending(q => q.TimeStamp).Take(10))
                {
                    result.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }


        public static IEnumerable<Post> PopularVideos()
        {
            db = new SwpEntities();
            try
            {
                return db.Posts.Where(p => p.Type == 2 && p.Enable == true).Take(2).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> MorePosts(string til)
        {
            db = new SwpEntities();
            try
            {
                if(til == "paper")
                {
                    return db.Posts.Where(p => p.Type == 3 && p.Enable == true).OrderByDescending(q => q.VisitCount).Take(4);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<File_DataFiles> MorePostsFiles(string til)
        {
            db = new SwpEntities();
            try
            {
                if (til == "book")
                {
                    return db.File_DataFiles.OrderByDescending(p => p.TimeStamp).Take(4);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static IEnumerable<Post> MediaForCategory(string cat)
        {
            db = new SwpEntities();
            try
            {
                int catid = db.Categories.Where(p => p.Title == cat && p.Enable == true).First().HierarchyId;
                return db.Posts.Where(p => p.CategoryId == catid && ( p.Type == 1 || p.Type == 2) && p.Enable == true).OrderByDescending(p => p.CreationDate).Take(1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> VideoMediaForCategory()
        {
            db = new SwpEntities();
            try
            {
                return db.Posts.Where(p => p.Type == 2 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> SeratMediaForCategory()
        {
            db = new SwpEntities();
            try
            {
                return db.Posts.Where(p => p.Type == 4 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Post> VoiceMediaForCategory()
        {
            db = new SwpEntities();
            try
            {
                return db.Posts.Where(p => p.Type == 1 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static IEnumerable<Post> PaperForCategory()
        {
            db = new SwpEntities();
            try
            {
                return db.Posts.Where(p => p.Type == 3 && p.Enable == true).OrderByDescending(q => q.CreationDate).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<File_DataFiles> BookForCategory()
        {
            db = new SwpEntities();
            try
            {
                return db.File_DataFiles.OrderByDescending(p => p.TimeStamp).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static IEnumerable<Post> MoreCategoryPost(int index,string category)
        {
            db = new SwpEntities();
            try
            {
                if (category == "مقاله")
                {
                    return db.Posts.Where(p => p.Type == 3 && p.Enable == true).OrderByDescending(q => q.CreationDate).Skip(index).Take(10);
                }
                else if (category == "فیلم")
                {
                    return db.Posts.Where(p => p.Type == 2 && p.Enable == true).OrderByDescending(q => q.CreationDate).Skip(index).Take(10);
                }
                else if (category == "صوت")
                {
                    return db.Posts.Where(p => p.Type == 1 && p.Enable == true).OrderByDescending(q => q.CreationDate).Skip(index).Take(10);
                }
                else if (category == "برنامه صراط مستقیم")
                {
                    return db.Posts.Where(p => p.Type == 4 && p.Enable == true).OrderByDescending(q => q.CreationDate).Skip(index).Take(10);
                }
                else
                {
                    return null;
                }
                //else if(category == "کتابخانه")
                //{
                //    return db.File_DataFiles.OrderByDescending(p => p.TimeStamp).Skip(index).Take(10);
                //}
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<File_DataFiles> MoreCategoryFile(int index)
        {
            db = new SwpEntities();
            try
            {
              return db.File_DataFiles.OrderByDescending(p => p.TimeStamp).Skip(index).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static Media_DataFiles FetchMedia(string filename)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type != 3 && p.Title == filename && p.Enable == true).First();
                return db.Media_DataFiles.Where(p => p.PostId == post.Id).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Media_DataFiles FetchSerat(string filename)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type == 4 && p.Title == filename && p.Enable == true).First();
                return db.Media_DataFiles.Where(p => p.PostId == post.Id).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<LocalVariable> FetchIndexLocals()
        {
            try
            {
                db = new SwpEntities();
                return db.LocalVariables.Where(p => p.Type == 0).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Paper_DataFiles FetchPaper(string title)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type == 3 && p.Title == title).First();
                return db.Paper_DataFiles.Where(p => p.PostId == post.Id).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static File_DataFiles FetchBook(string title)
        {
            try
            {
                db = new SwpEntities();
                return db.File_DataFiles.Where(p => p.Caption == title).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Media_DataFiles> MediaSimilars(string filename)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type != 3 && p.Title == filename).First();
                var keywords = db.Media_DataFiles.Where(p => p.PostId == post.Id).First().Keywords;
                return db.Media_DataFiles.Where(p => keywords.Contains(p.Keywords) && p.PostId != post.Id).Take(3).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Media_DataFiles> SeratSimilars(string filename)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type == 4 && p.Title == filename).First();
                var keywords = db.Media_DataFiles.Where(p => p.PostId == post.Id).First().Keywords;
                return db.Media_DataFiles.Where(p => keywords.Contains(p.Keywords) && p.PostId != post.Id).Take(3).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Paper_DataFiles> PaperSimilars(string filename)
        {
            try
            {
                db = new SwpEntities();
                var post = db.Posts.Where(p => p.Type == 3 && p.Title == filename).First();
                var keywords = db.Paper_DataFiles.Where(p => p.PostId == post.Id).First().Keywords;
                return db.Paper_DataFiles.Where(p => keywords.Contains(p.Keywords) && p.PostId != post.Id).Take(3).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Doubt> GetDoubts()
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(p => p.Type == true && p.Enable == true).OrderByDescending(p => p.TimeStamp).Take(10).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Doubt> MoreDoubt(int index)
        {
            db = new SwpEntities();
            try
            {
                    return db.Doubts.Where(p => p.Type == true && p.Enable == true).OrderByDescending(q => q.TimeStamp).Skip(index).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Doubt> GetQuestions()
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(q => q.Type == false && q.Enable == true).OrderByDescending(p => p.TimeStamp).Take(10).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Doubt FetchDoubt(string title)
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(p => p.Title == title && p.Enable == true).First();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static IEnumerable<Doubt> MoreQuestion(int index)
        {
            db = new SwpEntities();
            try
            {
                return db.Doubts.Where(p => p.Type == false && p.Enable == true).OrderByDescending(q => q.TimeStamp).Skip(index).Take(10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool SaveMessage(string name,string email,string message)
        {
            try
            {
                db = new SwpEntities();
                LocalVariable lv = new LocalVariable() { CreationDate = DateTime.Now, Id = Guid.NewGuid(), Title = name, Type = 2, Value=email + "," + message, IsNecessary = false };
                db.LocalVariables.Add(lv);
                if (db.SaveChanges() == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<Post> SearchThis(string keys)
        {
            List<Post> searchResult = new List<Post>();
            try
            {
                db = new SwpEntities();
                foreach (var searchinposts in db.Posts.Where(p => p.Title.Contains(keys) && p.Enable == true).ToList())
                {
                    searchResult.Add(searchinposts);
                }
                foreach (var mediakeywords in db.Media_DataFiles.Where(p => keys.Contains(p.Caption) || keys.Contains(p.Keywords)).ToList())
                {
                    searchResult.Add(mediakeywords.Post);
                }
                foreach (var paperkeywords in db.Paper_DataFiles.Where(p => keys.Contains(p.Title) || keys.Contains(p.Keywords)).ToList())
                {
                    searchResult.Add(paperkeywords.Post);
                }

            }
            catch (Exception)
            {
            }
            return searchResult;
        }

        public static string ToBase64ImageString(byte[] data)
        {
            return string.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(data));
        }

        public static string imgFileTostring(string fpath)
        {
            byte[] byt = System.IO.File.ReadAllBytes(fpath);
            return ToBase64ImageString(byt);
        }
    }
}