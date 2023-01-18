using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWProject.ViewModels.Front;
using SWProject.Helpers.Front;
using SWProject.Models.DomainModel;

namespace SWProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                IndexViewModel ivm = new IndexViewModel();
                ivm.slider = Utilities.IndexSlider();
                //ivm.LastPosts = Utilities.LastPosts();
                ivm.TopTypePosts = Utilities.TopTypePost();
                ivm.locals = Utilities.FetchIndexLocals();
                ivm.LastBooks = Utilities.LastFiles();
                ivm.LastPapers = Utilities.LastPapers();
                ivm.QnD = Utilities.Qnds();
                ivm.lastseratposts = Utilities.LastseratPosts();
                Loggings.TrackThis(Request, Response);
                return View(ivm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Categories(string Name)
        {
            try
            {
                CategoryViewModel cvm = new CategoryViewModel();
                if (Name == "videos")
                {
                    cvm.medias = Utilities.VideoMediaForCategory();
                    Name = "فیلم";
                }
                else if (Name == "voices")
                {
                    cvm.medias = Utilities.VoiceMediaForCategory();
                    Name = "صوت";
                }
                else if (Name == "papers")
                {
                    cvm.papers = Utilities.PaperForCategory();
                    Name = "مقاله";
                }
                else if (Name == "books")
                {
                    cvm.files = Utilities.BookForCategory();
                    Name = "کتابخانه";
                }
                else if (Name == "shows")
                {
                    cvm.serats = Utilities.SeratMediaForCategory();
                    Name = "برنامه صراط مستقیم";
                }
                cvm.ThisCategory = Name;
                Loggings.TrackThis(Request, Response);
                return View(cvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult MoreCategoryPost(int Current, string Category)
        {
            try
            {
                string viewname = "";
                Loggings.TrackThis(Request, Response);
                if (Category == "کتابخانه")
                {
                    var res1 = Utilities.MoreCategoryFile(Current);
                    viewname = "MoreCategoryFile";
                    return PartialView(viewname, res1);
                }
                else if (Category == "مقاله")
                {
                    var res2 = Utilities.MoreCategoryPost(Convert.ToInt32(Current), Category);
                    viewname = "MoreCategoryPostPaper";
                    return PartialView(viewname, res2);

                }
                else if (Category == "برنامه صراط مستقیم")
                {
                    var res2 = Utilities.MoreCategoryPost(Convert.ToInt32(Current), Category);
                    viewname = "MoreCategoryPostSerat";
                    return PartialView(viewname, res2);

                }
                else
                {
                    var res3 = Utilities.MoreCategoryPost(Convert.ToInt32(Current), Category);
                    viewname = "MoreCategoryPostMedia";
                    return PartialView(viewname, res3);
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public ActionResult Paper(string title)
        {
            try
            {
                PaperViewModel pvm = new PaperViewModel();
                pvm.paper = Utilities.FetchPaper(title);
                pvm.similar_paper = Utilities.PaperSimilars(title);
                Loggings.TrackThis(Request, Response);
                return View(pvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Video(string title)
        {
            try
            {
                MediaViewModel mvm = new MediaViewModel();
                mvm.media = Utilities.FetchMedia(title);
                mvm.similars = Utilities.MediaSimilars(title);
                Loggings.TrackThis(Request, Response);
                return View(mvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Show(string title)
        {
            try
            {
                MediaViewModel mvm = new MediaViewModel();
                mvm.serat = Utilities.FetchSerat(title);
                mvm.similars = Utilities.SeratSimilars(title);
                Loggings.TrackThis(Request, Response);
                return View(mvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Audio(string title)
        {
            try
            {
                MediaViewModel mvm = new MediaViewModel();
                mvm.media = Utilities.FetchMedia(title);
                mvm.similars = Utilities.MediaSimilars(title);
                Loggings.TrackThis(Request, Response);
                return View(mvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FileResult DownloadAudio(string title)
        {
            try
            {
                var audio = Utilities.FetchMedia(title);
                byte[] current = System.IO.File.ReadAllBytes(audio.FilePath.Replace("~", Server.MapPath("~")));
                Loggings.TrackThis(Request, Response);
                var response = new FileContentResult(current, "audio/mp3");
                response.FileDownloadName = audio.Caption;
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FileResult DownloadFilm(string title)
        {
            try
            {
                var film = Utilities.FetchMedia(title);
                byte[] current = System.IO.File.ReadAllBytes(film.FilePath.Replace("~", Server.MapPath("~")));
                Loggings.TrackThis(Request, Response);
                var response = new FileContentResult(current, "video/mp4");
                response.FileDownloadName = film.Caption;
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BookDetail(string title)
        {
            try
            {
                var book = Utilities.FetchBook(title);
                return View(book);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public FileResult Book(string title)
        {
            try
            {
                var book = Utilities.FetchBook(title);
                byte[] current = System.IO.File.ReadAllBytes(book.FilePath.Replace("~", Server.MapPath("~")));
                Loggings.TrackThis(Request, Response);
                var response = new FileContentResult(current, "application/pdf");
                response.FileDownloadName = book.Caption;
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Doubt()
        {
            try
            {
                var res = Utilities.GetDoubts();
                Loggings.TrackThis(Request, Response);
                return View(res);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult MoreDoubt(int Current)
        {
            try
            {
                string viewname = "";

                var res1 = Utilities.MoreDoubt(Current);
                viewname = "MoreDoubt";
                Loggings.TrackThis(Request, Response);
                return PartialView(viewname, res1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Question()
        {
            try
            {
                var res = Utilities.GetQuestions();
                Loggings.TrackThis(Request, Response);
                return View(res);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DoubtDetail(string title)
        {
            try
            {
                var res3 = Utilities.FetchDoubt(title);
                Loggings.TrackThis(Request, Response);
                return View(res3);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult QuestionDetail(string title)
        {
            try
            {
                var res4 = Utilities.FetchDoubt(title);
                Loggings.TrackThis(Request, Response);
                return View(res4);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult MoreQuestion(int Current)
        {
            try
            {
                string viewname = "";

                var res1 = Utilities.MoreQuestion(Current);
                viewname = "MoreQuestion";
                Loggings.TrackThis(Request, Response);
                return PartialView(viewname, res1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ContactUs()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult SendMessage(string Name,string Email,string Message)
        {
            try
            {
                if (Utilities.SaveMessage(Name,Email,Message))
                {
                    Loggings.TrackThis(Request, Response);
                    return Json(new JsonData()
                    {
                        isDone = true,
                        Message = "پیغام شما با موفقیت ثبت شد"
                    });
                }                    
                else
                {
                    Loggings.TrackThis(Request, Response);
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا بعدا امتحان کنید"
                    });
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ActionResult MorePost(string toload)
        {
            try
            {
                var res = Utilities.MorePosts(toload);
                Loggings.TrackThis(Request, Response);
                return PartialView(res);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult MorePostFiles(string toload)
        {
            try
            {
                var res = Utilities.MorePostsFiles(toload);
                Loggings.TrackThis(Request, Response);
                return PartialView(res);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult SearchAsYouType(string fkey)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fkey))
                {
                    var getresult = Utilities.SearchThis(fkey);
                    Loggings.TrackThis(Request, Response);
                    return PartialView("_SearchResultAsYouType", getresult);
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

        public ActionResult SearchResult(string fkey)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fkey))
                {
                    var getresult = Utilities.SearchThis(fkey);
                    Loggings.TrackThis(Request, Response);
                    return View("SearchResult", getresult);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult CommonItems()
        {
            string insta = Utilities.getInsta();
            if(insta != null)
            return Json(new JsonData() { isDone = true,Message=insta });
            else
                return Json(new JsonData() { isDone = false, Message = "#" });
        }

        public class JsonData
        {
            public bool isDone { get; set; }
            public string Message { get; set; }
        }
    }
}
