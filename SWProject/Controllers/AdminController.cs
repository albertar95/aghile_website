using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using SWProject.Helpers.Admin;
using SWProject.Models.DomainModel;
using SWProject.ViewModels.Admin;
using System.Web.Security;
using System.IO;

namespace SWProject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
            }
            catch (Exception)
            {
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Authen(string usrname, string pass)
        {
            try
            {
                User adm = Utilities.Authenticate(usrname, pass);
                if (adm != null)
                {
                    FormsAuthentication.SetAuthCookie(usrname, false);
                    if(adm.Grade == 1)
                    TempData["currentUserid"] = "gradevi";
                    if (adm.Grade == 2)
                        TempData["currentUserid"] = "gradeiv";
                    TempData["currentUser"] = adm.Name;
                    return Redirect("~/swp/admin/Index");
                    //return Redirect("~/swp/admin/index");
                }
                else
                {
                    TempData["notSuccessfulLogin"] = "نام کاربری یا کلمه عبور صحیح نمی باشد";
                    return View("Login");
                }
            }
            catch (Exception)
            {
                TempData["notSuccessfulLogin"] = "مشکل در برنامه.لطفا مجددا امتحان کنید";
                return View("Login");
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/swp/admin/Index");
        }


        // GET: Admin
            
        public ActionResult Index()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                //var x = Guid.Parse(TempData["currentUser"].ToString());
                //get user
            }
            catch (Exception)
            {
            }
            return View();
        }


        //category section
        public ActionResult Categories()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetCategories();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddCategory()
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

        public ActionResult EditCategory()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetCategories();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteCategory()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetCategories();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public ActionResult SubmitAddCategory(Category toadd,HttpPostedFileBase ProfilePicId)
        {
            try
            {
                var res = Utilities.SubmitAddCategory(toadd, ProfilePicId);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulCategory"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulCategory"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Categories");
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteCategory(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteCategory(Convert.ToInt32(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }
        public ActionResult SubmitEditCategory(Category toedit,HttpPostedFileBase ProfilePicId)
        {
            try
            {
                var res = Utilities.SubmitEditCategory(toedit, ProfilePicId);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulCategory"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulCategory"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Categories");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditCategoryForm(int gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var catEdit = Utilities.fetchCategory(gid);
                if (catEdit.HierarchyParent == 0)
                    catEdit.HasParent = false;
                return View(catEdit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AdminStatus()
        {
            try
            {
                if (Utilities.FetchUserByName(User.Identity.Name).Grade == 2)
                    return Json(new JsonData() { isDone = true });
                else
                    return Json(new JsonData() { isDone = false });
            }
            catch (Exception)
            {
                return null;
            }
        }

        //users section

        public ActionResult Users()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var users = Utilities.GetUsers();
                return View(users);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ActionResult UserDetails(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var user = Utilities.FetchUser(Guid.Parse(id));
                return PartialView("_UserInformationPartialView", user);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult AddUser()
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
        public ActionResult DeleteUser()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetUsers();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult SubmitAddUser(User toadd, HttpPostedFileBase ProfilePicId)
        {
            try
            {
                var res = Utilities.SubmitAddUser(toadd, ProfilePicId);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulUser"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulUser"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Users");
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteUser(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteUser(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        //local variables section

        public ActionResult Locals()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetLocals();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddLocal()
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

        public ActionResult EditLocal()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetLocals();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteLocal()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetLocals();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public ActionResult SubmitAddLocal(LocalVariable toadd)
        {
            try
            {
                    var res = Utilities.SubmitAddLocal(toadd);
                    if (Convert.ToBoolean(res[0]))
                        //ViewBag.Successful = res[1];
                        TempData["SuccessfulLocal"] = res[1];
                    else
                        //ViewBag.notSuccessful = res[1];
                        TempData["notSuccessfulLocal"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Locals");
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteLocal(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteLocal(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }
        public ActionResult SubmitEditLocal(LocalVariable toedit)
        {
            try
            {
                    var res = Utilities.SubmitEditLocal(toedit);
                    if (Convert.ToBoolean(res[0]))
                        //ViewBag.Successful = res[1];
                        TempData["SuccessfulLocal"] = res[1];
                    else
                        //ViewBag.notSuccessful = res[1];
                        TempData["notSuccessfulLocal"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Locals");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditLocalForm(Guid gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var catEdit = Utilities.fetchLocal(gid);
                return View(catEdit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //user message section


        public ActionResult UserMessages()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetUserMessages();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteUserMessage()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetUserMessages();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //[HttpPost]
        public ActionResult SubmitDeleteUserMessage(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteUserMessage(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        public ActionResult UserMessageDetails(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var userM = Utilities.fetchUserMessage(Guid.Parse(id));
                return PartialView("_UserMessagePartialView", userM);
            }
            catch (Exception)
            {
                return null;
            }
        }


        //posts section
        public ActionResult Posts()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetPosts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult PostDetail(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                Post post = Utilities.FetchPost(Guid.Parse(id));
                PostDetailViewModel pdvm = new PostDetailViewModel();
                pdvm.post = post;
                if (post.Type == 3)
                {
                    pdvm.paper = Utilities.FetchPaper(post.Id);
                }
                else
                {
                    pdvm.media = Utilities.FetchMedia(post.Id);
                }
                return View(pdvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddPost()
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

        public ActionResult AddPost_Film()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                return PartialView("_AddPostFilm");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddPost_Voice()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                return PartialView("_AddPostVoice");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddPost_Paper()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                return View("_AddPostPaper");
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        [ValidateInput(false)]
        public ActionResult SubmitAddPaperPost(PaperPostViewModel ppvm,HttpPostedFileBase ProfilePicId)
        {
            try
            {
                Post currentPost = new Post();
                currentPost = ppvm.Post;
                var res1 = Utilities.AddPost(currentPost, ProfilePicId, 3);
                if (Convert.ToBoolean(res1[0]))
                {
                    Paper_DataFiles currentPaper = new Paper_DataFiles();
                    currentPaper = ppvm.Paper;
                    var res2 = Utilities.AddPaper(currentPaper, Guid.Parse(res1[1].ToString()));
                    if (Convert.ToBoolean(res2[0]))
                        TempData["SuccessfulPost"] = res2[1];
                    else
                        TempData["notSuccessfulPost"] = res2[1];
                }
                return RedirectToAction("Posts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitAddFilmMediaPost(MediaPostViewModel mpvm, HttpPostedFileBase ProfilePicId)
        {
            try
            {
                Post currentPost = new Post();
                currentPost = mpvm.post;
                var res1 = Utilities.AddPost(currentPost, ProfilePicId, 2);
                if (Convert.ToBoolean(res1[0]))
                {
                    //Helpers.Utility.Utility.ResizeImageByWidth(CoverImage.InputStream, 200, path, Helpers.Utility.Utility.ImageComperssion.Good);
                    Media_DataFiles currentFilm = new Media_DataFiles();
                    currentFilm = mpvm.media;
                    string[] str = new string[4];
                    int i = 0;
                    foreach (var item in mpvm.media.FilePath.Split(','))
                    {
                        str.SetValue(item, i);
                        i++;
                    }
                    currentFilm.FileName = str[2];
                    currentFilm.FileExtention = str[1];
                    currentFilm.Size = int.Parse(str[3].Remove(str[3].Length - 1, 1));
                    currentFilm.FilePath = str[0].Remove(0, 1);
                    var res2 = Utilities.AddMedia(currentFilm, Guid.Parse(res1[1].ToString()), true);
                    if (Convert.ToBoolean(res2[0]))
                        TempData["SuccessfulPost"] = res2[1];
                    else
                        TempData["notSuccessfulPost"] = res2[1];
                }
                return RedirectToAction("Posts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UploadFilm()
        {
            try
            {
                string path = "";
                string mimeType = "";
                string fileName = "";
                int fileSize = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    fileSize = file.ContentLength;
                    fileName = file.FileName;
                    mimeType = file.ContentType;

                    //path = Server.MapPath("~") + "Files\\" + "Films\\" + fileName;
                    path = Server.MapPath("~")  +"Files\\" + "Films\\" + fileName;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(path); //File will be saved in application root
                }
                return Json(path + "," + mimeType + "," + fileName + "," + fileSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitAddVoiceMediaPost(MediaPostViewModel mpvm,HttpPostedFileBase ProfilePicId)
        {
            try
            {
                Post currentPost = new Post();
                currentPost = mpvm.post;
                var res1 = Utilities.AddPost(currentPost, ProfilePicId, 1);
                if (Convert.ToBoolean(res1[0]))
                {
                    //Helpers.Utility.Utility.ResizeImageByWidth(CoverImage.InputStream, 200, path, Helpers.Utility.Utility.ImageComperssion.Good);
                    Media_DataFiles currentvoice = new Media_DataFiles();
                    currentvoice = mpvm.media;
                    string[] str = new string[4];
                    int i = 0;
                    foreach (var item in mpvm.media.FilePath.Split(','))
                    {
                        str.SetValue(item, i);
                        i++;
                    }
                    currentvoice.FileName = str[2];
                    currentvoice.FileExtention = str[1];
                    currentvoice.Size = int.Parse(str[3].Remove(str[3].Length - 1, 1));
                    currentvoice.FilePath = str[0].Remove(0, 1);
                    var res2 = Utilities.AddMedia(currentvoice, Guid.Parse(res1[1].ToString()), false);
                    if (Convert.ToBoolean(res2[0]))
                        TempData["SuccessfulPost"] = res2[1];
                    else
                        TempData["notSuccessfulPost"] = res2[1];
                }
                return RedirectToAction("Posts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UploadVoice()
        {
            try
            {
                string path = "";
                string mimeType = "";
                string fileName = "";
                int fileSize = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    fileSize = file.ContentLength;
                    fileName = file.FileName;
                    mimeType = file.ContentType;

                    path = Server.MapPath("~") + "Files\\" + "Voices\\" + fileName;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(path); //File will be saved in application root
                }
                return Json(path + "," + mimeType + "," + fileName + "," + fileSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditPost()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetPosts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeletePost()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetPosts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditPostForm(Guid gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var postEdit = Utilities.FetchPost(gid);
                EditPostViewModel epvm = new EditPostViewModel();
                epvm.post = postEdit;
                if (postEdit.Type == 3)
                {
                    var paperEdit = Utilities.FetchPaper(postEdit.Id);
                    epvm.paper = paperEdit;
                }
                if (postEdit.Type == 2 || postEdit.Type == 1)
                {
                    var mediaEdit = Utilities.FetchMedia(postEdit.Id);
                    epvm.media = mediaEdit;
                }
                return View(epvm);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult EditPostFormMedia(Guid gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var postEdit = Utilities.FetchPost(gid);
                EditPostViewModel epvm = new EditPostViewModel();
                epvm.post = postEdit;
                if (postEdit.Type == 3)
                {
                    var paperEdit = Utilities.FetchPaper(postEdit.Id);
                    epvm.paper = paperEdit;
                }
                if (postEdit.Type == 2 || postEdit.Type == 1)
                {
                    var mediaEdit = Utilities.FetchMedia(postEdit.Id);
                    epvm.media = mediaEdit;
                }
                return View(epvm);
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult SubmitDeletePost(string CurrentId)
        {
            try
            {
                Guid g2 = Guid.Parse(CurrentId);
                var g1 = Utilities.FetchPost(g2);
                string pp = g1.Media_DataFiles.Where(p => p.PostId == g2).First().FilePath.Replace("~", Server.MapPath("~"));
                    if (System.IO.File.Exists(pp))
                {
                    System.IO.File.Delete(pp);
                }
                var getresult = Utilities.SubmitDeletePost(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitEditPost(EditPostViewModel toedit,HttpPostedFileBase ProfilePicId)
        {
            try
            {
                var res = Utilities.SubmitEditPost(toedit,ProfilePicId);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulPost"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulPost"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Posts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Books section

        public ActionResult Books()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetBooks();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BookDetail(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                File_DataFiles book = Utilities.FetchBook(Guid.Parse(id));
                return View(book);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddBook()
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

        [ValidateInput(false)]
        public ActionResult SubmitAddBook(File_DataFiles book,HttpPostedFileBase profilepic)
        {
            try
            {
                Guid imggid = Guid.Empty;
                if(profilepic != null)
                {
                    var respic = Helpers.Admin.Utilities.AddImage(profilepic);
                    if(Convert.ToBoolean(respic[0]) == true)
                    {
                        imggid = Guid.Parse(respic[1].ToString());
                    }
                }
                string[] str = new string[4];
                int i = 0;
                foreach (var item in book.FilePath.Split(','))
                {
                    str.SetValue(item, i);
                    i++;
                }
                book.ImgId = imggid;
                book.FileName = str[2];
                book.FileExtention = str[1];
                book.Size = int.Parse(str[3].Remove(str[3].Length - 1, 1));
                book.FilePath = str[0].Remove(0, 1);
                var res2 = Utilities.AddBook(book);
                if (Convert.ToBoolean(res2[0]))
                    TempData["SuccessfulBook"] = res2[1];
                else
                    TempData["notSuccessfulBook"] = res2[1];
                return RedirectToAction("Books");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UploadBook()
        {
            try
            {
                string path = "";
                string mimeType = "";
                string fileName = "";
                int fileSize = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    fileSize = file.ContentLength;
                    fileName = file.FileName;
                    mimeType = file.ContentType;

                    path = Server.MapPath("~") + "Files\\" + "Books\\" + fileName;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(path); //File will be saved in application root
                }
                return Json(path + "," + mimeType + "," + fileName + "," + fileSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditBook()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetBooks();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteBook()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetBooks();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditBookForm(Guid gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var bookEdit = Utilities.FetchBook(gid);
                return View(bookEdit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteBook(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteBook(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitEditBook(File_DataFiles book)
        {
            try
            {
                var res = Utilities.SubmitEditBook(book);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulBook"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulBook"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Books");
            }
            catch (Exception)
            {
                return null;
            }
        }

        //douts section
        public ActionResult Doubts()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetDoubts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DoubtDetail(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var doubt = Utilities.FetchDoubt(Guid.Parse(id));
                return View(doubt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditDoubt()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetDoubts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteDoubt()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetDoubts();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddDoubt()
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

        [ValidateInput(false)]
        public ActionResult EditDoubtForm(Guid did)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var DoubtEdit = Utilities.FetchDoubt(did);
                return View(DoubtEdit);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public ActionResult SubmitDeleteDoubt(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteDoubt(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitEditDoubt(Doubt toedit)
        {
            try
            {
                var res = Utilities.SubmitEditDoubt(toedit);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulDoubt"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulDoubt"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Doubts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitAddDoubt(Doubt doubt)
        {

            try
            {
                var res1 = Utilities.AddDoubt(doubt);
                if (Convert.ToBoolean(res1[0]))
                {
                    TempData["SuccessfulDoubt"] = res1[1];
                }
                else
                    TempData["notSuccessfulDoubt"] = res1[1];

                return RedirectToAction("Doubts");
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Questions
        public ActionResult Questions()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetQuestions();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult QuestionDetail(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var Question = Utilities.FetchQuestion(Guid.Parse(id));
                return View(Question);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditQuestion()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetQuestions();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteQuestion()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetQuestions();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddQuestion()
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

        [ValidateInput(false)]
        public ActionResult EditQuestionForm(Guid did)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var QuestionEdit = Utilities.FetchQuestion(did);
                return View(QuestionEdit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult SubmitDeleteQuestion(string CurrentId)
        {
            try
            {
                var getresult = Utilities.SubmitDeleteQuestion(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitEditQuestion(Doubt toedit)
        {
            try
            {
                var res = Utilities.SubmitEditQuestion(toedit);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulQuestion"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulQuestion"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Questions");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitAddQuestion(Doubt doubt)
        {

            try
            {
                var res1 = Utilities.AddQuestion(doubt);
                if (Convert.ToBoolean(res1[0]))
                {
                    TempData["SuccessfulQuestion"] = res1[1];
                }
                else
                    TempData["notSuccessfulQuestion"] = res1[1];

                return RedirectToAction("Questions");
            }
            catch (Exception)
            {
                return null;
            }
        }

        //slider

        public ActionResult Sliders()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Utilities.GetSliders();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult AddSlider()
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
        public ActionResult DeleteSlider()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var list = Helpers.Admin.Utilities.GetSliders();
                return View(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult SubmitAddSlider(LocalVariable toadd,HttpPostedFileBase picture)
        {
            try
            {
                string path = Server.MapPath("~") + "Files\\" + "Slider\\" + picture.FileName;
                System.IO.Stream fileContent = picture.InputStream;
                picture.SaveAs(path);
                toadd.Value = Convert.ToString("~" + "/" + "Files" + "/" + "Slider" + "/" + picture.FileName.ToString());
                toadd.IsNecessary = false;
                toadd.Type = 3;
                var res = Utilities.SubmitAddSlider(toadd);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulSlider"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulSlider"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Sliders");
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteSlider(string CurrentId)
        {
            try
            {
                var theslider = Utilities.fetchLocal(Guid.Parse(CurrentId));
                theslider.Value = theslider.Value.Replace("~", Server.MapPath("~"));
                if(System.IO.File.Exists(theslider.Value))
                {
                    System.IO.File.Delete(theslider.Value);
                }
                var getresult = Utilities.SubmitDeleteSlider(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }
        public ActionResult Logs()
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                LogsViewModel lvm = new LogsViewModel();
                lvm.PagesBarChart = Utilities.CloneBarChartData(Utilities.StatPagesBarChart());
                return View(lvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult simpleLogs()
        {
            try
            {
                var res = Utilities.SimpleLogs();
                return View(res);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public PartialViewResult GalleryManagement()
        {
            try
            {
                return PartialView();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult UploadGalleryPic()
        {
            try
            {
                var file = Request.Files[0];
                string path = Server.MapPath("~") + "Files\\" + "Gallery\\" + file.FileName;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(path);
                string Value = Convert.ToString(Request.Url.GetLeftPart(UriPartial.Authority) + "/" + "Files" + "/" + "Gallery" + "/" + file.FileName.ToString());
                return Json(new JsonData()
                {
                    isDone = Convert.ToBoolean(true),
                    Message = Value.ToString()
                });
            }
            catch (Exception)
            {
                return null;
            }

        }

        public ActionResult FetchGalleryFolder()
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~") + "Files\\" + "Gallery");
                FileInfo[] ffj = d.GetFiles("*.jpg");
                FileInfo[] ffp = d.GetFiles("*.png");
                string str = "";
                foreach (var item in ffj)
                {
                    str = str + Convert.ToString(Request.Url.GetLeftPart(UriPartial.Authority) + "/" + "Files" + "/" + "Gallery" + "/" + item.Name.ToString()) + ",";
                }
                foreach (var item in ffp)
                {
                    str = str + Convert.ToString(Request.Url.GetLeftPart(UriPartial.Authority) + "/" + "Files" + "/" + "Gallery" + "/" + item.Name.ToString()) + ",";
                }
                return PartialView("_FetchGalleryFolder", str.Substring(0, str.Length - 1));
            }
            catch (Exception)
            {
                return null;
            }
        }


        //serat

        public ActionResult Serats()
        {
            Loggings.TrackThis(Request, Response);
            var list = Utilities.GetSerats();
            return View(list);
        }

        public ActionResult EditSerats()
        {
            Loggings.TrackThis(Request, Response);
            var list = Utilities.GetSerats();
            return View(list);
        }

        public ActionResult SeratDetail(string id)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                Post post = Utilities.FetchPost(Guid.Parse(id));
                PostDetailViewModel pdvm = new PostDetailViewModel();
                pdvm.post = post;
                if (post.Type == 4)
                {
                    pdvm.serat = Utilities.FetchSerat(post.Id);
                }
                return View(pdvm);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult DeleteSerats()
        {
            Loggings.TrackThis(Request, Response);
            var list = Utilities.GetSerats();
            return View(list);
        }

        public ActionResult AddSerats()
        {
            Loggings.TrackThis(Request, Response);
            return View();
        }

        public JsonResult UploadSeratFilm()
        {
            try
            {
                string path = "";
                string mimeType = "";
                string fileName = "";
                int fileSize = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    fileSize = file.ContentLength;
                    fileName = file.FileName;
                    mimeType = file.ContentType;

                    path = Server.MapPath("~") + "Files\\" + "Serat\\" + fileName;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(path); //File will be saved in application root
                }
                return Json(path + "," + mimeType + "," + fileName + "," + fileSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult SubmitAddSerats(MediaPostViewModel mpvm, HttpPostedFileBase ProfilePicId)
        {
            try
            {
                Post currentPost = new Post();
                currentPost = mpvm.post;
                var res1 = Utilities.AddPost(currentPost, ProfilePicId, 4);
                if (Convert.ToBoolean(res1[0]))
                {
                    //Helpers.Utility.Utility.ResizeImageByWidth(CoverImage.InputStream, 200, path, Helpers.Utility.Utility.ImageComperssion.Good);
                    Media_DataFiles currentFilm = new Media_DataFiles();
                    currentFilm = mpvm.media;
                    string[] str = new string[4];
                    int i = 0;
                    foreach (var item in mpvm.media.FilePath.Split(','))
                    {
                        str.SetValue(item, i);
                        i++;
                    }
                    currentFilm.FileName = str[2];
                    currentFilm.FileExtention = str[1];
                    currentFilm.Size = int.Parse(str[3].Remove(str[3].Length - 1, 1));
                    currentFilm.FilePath = str[0].Remove(0, 1);
                    var res2 = Utilities.AddSerat(currentFilm, Guid.Parse(res1[1].ToString()), true);
                    if (Convert.ToBoolean(res2[0]))
                        TempData["SuccessfulPost"] = res2[1];
                    else
                        TempData["notSuccessfulPost"] = res2[1];
                }
                return RedirectToAction("Serats");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EditSeratForm(Guid gid)
        {
            try
            {
                Loggings.TrackThis(Request, Response);
                var postEdit = Utilities.FetchPost(gid);
                EditPostViewModel epvm = new EditPostViewModel();
                epvm.post = postEdit;
                if (postEdit.Type == 4)
                {
                    var mediaEdit = Utilities.FetchSerat(postEdit.Id);
                    epvm.serat = mediaEdit;
                }
                return View(epvm);
            }
            catch (Exception)
            {
                return null;
            }
        }


        [ValidateInput(false)]
        public ActionResult SubmitEditSerats(EditPostViewModel toedit, HttpPostedFileBase ProfilePicId)
        {
            try
            {
                var res = Utilities.SubmitEditPost(toedit, ProfilePicId);
                if (Convert.ToBoolean(res[0]))
                    //ViewBag.Successful = res[1];
                    TempData["SuccessfulPost"] = res[1];
                else
                    //ViewBag.notSuccessful = res[1];
                    TempData["notSuccessfulPost"] = res[1];
                //var list = Helpers.Admin.Utilities.GetCategories();
                return RedirectToAction("Serats");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult SubmitDeleteSerats(string CurrentId)
        {
            try
            {
                Guid g2 = Guid.Parse(CurrentId);
                var g1 = Utilities.FetchPost(g2);
                string pp = g1.Media_DataFiles.Where(p => p.PostId == g2).First().FilePath.Replace("~", Server.MapPath("~"));
                if (System.IO.File.Exists(pp))
                {
                    System.IO.File.Delete(pp);
                }
                var getresult = Utilities.SubmitDeleteSerat(Guid.Parse(CurrentId));
                if (getresult != null)
                {
                    return Json(new JsonData()
                    {
                        isDone = Convert.ToBoolean(getresult[0]),
                        Message = getresult[1].ToString()
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        isDone = false,
                        Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new JsonData()
                {
                    isDone = false,
                    Message = "مشکل در برنامه.لطفا به راهبر سیستم اطلاع دهید"
                });
            }
        }

        public class JsonData
        {
            public bool isDone { get; set; }
            public string Message { get; set; }
        }
    }
}
