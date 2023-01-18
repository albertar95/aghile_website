using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWProject.Models.DomainModel;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using SWProject.Helpers.Admin;
using SWProject.ViewModels.Admin;
using System.IO;
using System.Globalization;

namespace SWProject.Helpers.Admin
{
    public class Utilities
    {

        static SwpEntities db = null;
        static string hashkey { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        static ArrayList submitResult = null;

        static ArrayList submitResult1 = null;



        public static User Authenticate(string user, string pass)
        {
            try
            {
                db = new SwpEntities();
                string enteredpass = Encrypt(pass);
                return db.Users.Where(p => p.UserName == user && p.Password == enteredpass).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }


        //category section
        public static IEnumerable<Category> GetCategories()
        {
            try
            {
                db = new SwpEntities();
                return db.Categories.Where(p => p.HierarchyId != 0).OrderBy(p => p.HierarchyId).ToList();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static Category fetchCategory(int Hid)
        {
            try
            {
                db = new SwpEntities();
                var cat = db.Categories.Where(p => p.HierarchyId == Hid).ToList().First();
                return cat;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Category> rootCategories()

        {
            try
            {
                db = new SwpEntities();
                return db.Categories.Where(p => p.HierarchyParent == 0).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<Category> GetChildCategories(int parentId)
        {
            try
            {
                db = new SwpEntities();
                return db.Categories.Where(p => p.HasParent == true && p.HierarchyParent == parentId).OrderBy(p => p.HierarchyId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitDeleteCategory(int Hid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                bool checkpost = false;
                var currentCategory = db.Categories.Where(p => p.HierarchyId == Hid).First();
                var subs = db.Categories.Where(p => p.HierarchyParent == Hid).ToList();
                    if (subs.Count != 0)
                    {
                        submitResult.Add(false);
                        submitResult.Add("دسته بندی دارای زیر مجموعه می باشد");
                    }
                    else
                    {
                        checkpost = true;
                    }
                if (checkpost)
                {
                    var posts = db.Posts.Where(p => p.CategoryId == currentCategory.HierarchyId).ToList();
                    bool canDelete = false;
                    if (posts.Count != 0)
                    {
                        submitResult.Add(false);
                        submitResult.Add("این دسته بندی دارای پست می باشد"); 
                    }
                    else
                    {
                        if (currentCategory.ProfilePicId != null)
                        {
                            var img = db.Img_DataFiles.Where(p => p.Id == currentCategory.ProfilePicId).First();
                            db.Entry(img).State = System.Data.Entity.EntityState.Deleted;
                            db.SaveChanges();
                            currentCategory.ProfilePicId = null;
                            db.Entry(currentCategory).Property("ProfilePicId").IsModified = true;
                            db.SaveChanges();
                            canDelete = true;
                        }
                        else
                        {
                            canDelete = true;
                        }
                    }
                    if(canDelete)
                    {
                        db.Entry(currentCategory).State = System.Data.Entity.EntityState.Deleted;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("دسته بندی با موفقیت حذف شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitAddCategory(Category current,HttpPostedFileBase pic)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                if (pic != null)
                {
                    var res = AddImage(pic);
                    if (Convert.ToBoolean(res[0]))
                    {
                        current.ProfilePicId = Guid.Parse(res[1].ToString());
                    }
                }
                        current.HierarchyId = db.Categories.OrderByDescending(p => p.HierarchyId).First().HierarchyId + 1;
                        current.TimeStamp = DateTime.Now;
                        current.Id = Guid.NewGuid();
                        db.Categories.Add(current);
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("دسته بندی با موفقیت اضافه شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    
                
                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }

        public static ArrayList SubmitEditCategory(Category current,HttpPostedFileBase pic)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                current.TimeStamp = DateTime.Now;
                if (!current.HasParent)
                {
                    current.HasParent = true;
                    current.HierarchyParent = 0;
                }
                if (pic != null)
                {
                    var res = AddImage(pic);
                    if (Convert.ToBoolean(res[0]))
                    {
                        current.ProfilePicId = Guid.Parse(res[1].ToString());
                    }
                }
                    db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() == 1)
                    {
                        submitResult.Add(true);
                        submitResult.Add("دسته بندی با موفقیت ویرایش شد");
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
            }
            return submitResult;
        }

        //user section
        public static IEnumerable<User> GetUsers()
        {
            try
            {
                db = new SwpEntities();
                return db.Users.Where(p => p.Grade != 0).OrderBy(p => p.UserName).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static User FetchUserByName(string username)
        {
            try
            {
                db = new SwpEntities();
                return db.Users.Where(p => p.UserName == username).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitAddUser(User current,HttpPostedFileBase pic)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                var checkexist = db.Users.Where(p => p.UserName == current.UserName.ToString()).FirstOrDefault();
                if(checkexist == null)
                { 
                current.Id = Guid.NewGuid();
                current.Password = Encrypt(current.Password);
                if (pic != null)
                {
                    var res = AddImage(pic);
                    if (Convert.ToBoolean(res[0]))
                    {
                        current.ProfilePicId = Guid.Parse(res[1].ToString());
                    }
                }
                db.Users.Add(current);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("کاربر با موفقیت ایجاد شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("این نام کاربری موجود می باشد");
                }
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
            }
            return submitResult;
        }

        public static ArrayList SubmitDeleteUser(Guid gid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                bool checkdout = false;
                var currentposts = db.Posts.Where(p => p.UserId == gid).ToList();
                if (currentposts.Count != 0)
                {
                    submitResult.Add(false);
                    submitResult.Add("کاربر دارای پست می باشد");
                }
                else
                {
                    checkdout = true;
                }
                var user = db.Users.Where(p => p.Id == gid).First();
                if (checkdout)
                {
                    var douts = db.Doubts.Where(p => p.ExteriorId == gid).ToList();
                    bool canDelete = false;
                    if (douts.Count != 0)
                    {
                        submitResult.Add(false);
                        submitResult.Add("این کاربر دارای شبهه می باشد");
                    }
                    else
                    {
                        if (user.ProfilePicId != null)
                        {
                            var img = db.Img_DataFiles.Where(p => p.Id == user.ProfilePicId).First();
                            db.Entry(img).State = System.Data.Entity.EntityState.Deleted;
                            if (db.SaveChanges() == 1)
                            {
                                user.ProfilePicId = null;
                                db.Entry(user).Property("ProfilePicId").IsModified = true;
                                db.SaveChanges();
                                canDelete = true;
                            }
                            else
                            {
                                submitResult.Add(false);
                                submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                            }

                        }
                        else
                        {
                            canDelete = true;
                        }
                    }
                    if (canDelete)
                    {
                        db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("کاربر با موفقیت حذف شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static ArrayList SubmitEditUser(User current, Guid picguid)
        //{
        //    try
        //    {
        //        db = new SwpEntities();
        //        submitResult = new ArrayList();
        //        if (!current.HasParent)
        //        {
        //            current.HasParent = true;
        //            current.HierarchyParent = 0;
        //        }
        //        if (picguid != Guid.Empty)
        //            current.ProfilePicId = picguid;
        //        db.Entry(current).State = System.Data.Entity.EntityState.Modified;
        //        if (db.SaveChanges() == 1)
        //        {
        //            submitResult.Add(true);
        //            submitResult.Add("دسته بندی با موفقیت ویرایش شد");
        //        }
        //        else
        //        {
        //            submitResult.Add(false);
        //            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
        //        }
        //        return submitResult;
        //    }
        //    catch (Exception)
        //    {
        //        submitResult.Add(false);
        //        submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
        //        return submitResult;
        //    }
        //}


        //local variables section

        public static IEnumerable<LocalVariable> GetLocals()
        {
            try
            {
                db = new SwpEntities();
                return db.LocalVariables.Where(p => p.Type == 0).OrderBy(p => p.Id).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static LocalVariable fetchLocal(Guid Lid)
        {
            try
            {
                db = new SwpEntities();
                var cat = db.LocalVariables.Where(p => p.Id == Lid).ToList().First();
                return cat;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static List<Category> rootCategories()

        //{
        //    try
        //    {
        //        db = new SwpEntities();
        //        return db.Categories.Where(p => p.HierarchyParent == 0).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        //public static List<Category> GetChildCategories(int parentId)
        //{
        //    try
        //    {
        //        db = new SwpEntities();
        //        return db.Categories.Where(p => p.HasParent == true && p.HierarchyParent == parentId).OrderBy(p => p.HierarchyId).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public static ArrayList SubmitDeleteLocal(Guid Lid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentlocal = db.LocalVariables.Where(p => p.Id == Lid).First();
                if (currentlocal.IsNecessary)
                {
                    submitResult.Add(false);
                    submitResult.Add("این متغییر فقط قابل ویرایش می باشد");
                }
                else
                {
                    db.Entry(currentlocal).State = System.Data.Entity.EntityState.Deleted;
                    if (db.SaveChanges() == 1)
                    {
                        submitResult.Add(true);
                        submitResult.Add("متغییر با موفقیت حذف شد");
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitAddLocal(LocalVariable current)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                current.CreationDate = DateTime.Now;
                current.Id = Guid.NewGuid();
                db.LocalVariables.Add(current);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("متغییر با موفقیت اضافه شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }

        public static ArrayList SubmitEditLocal(LocalVariable current)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                current.ModifiedDate = DateTime.Now;
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("متغییر با موفقیت ویرایش شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }


        //user messages section

        public static IEnumerable<LocalVariable> GetUserMessages()
        {
            try
            {
                db = new SwpEntities();
                return db.LocalVariables.Where(p => p.Type == 2).OrderByDescending(p => p.CreationDate).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static LocalVariable fetchUserMessage(Guid Lid)
        {
            try
            {
                db = new SwpEntities();
                var cat = db.LocalVariables.Where(p => p.Id == Lid).ToList().First();
                return cat;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ArrayList SubmitDeleteUserMessage(Guid Lid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentlocal = db.LocalVariables.Where(p => p.Id == Lid).First();
                db.Entry(currentlocal).State = System.Data.Entity.EntityState.Deleted;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("پیام کاربر با موفقیت حذف شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //post section

        public static IEnumerable<Post> GetPosts()
        {
            try
            {
                db = new SwpEntities();
                return db.Posts.OrderBy(p => p.Type).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Post FetchPost(Guid guid)
        {
            try
            {
                db = new SwpEntities();
                return db.Posts.Where(p => p.Id == guid).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static User FetchUser(Guid guid)
        {
            try
            {
                db = new SwpEntities();
                return db.Users.Where(p => p.Id == guid).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Paper_DataFiles FetchPaper(Guid gid)
        {
            try
            {
                db = new SwpEntities();
                var paper = db.Paper_DataFiles.Where(p => p.PostId == gid).ToList().First();
                return paper;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static Media_DataFiles FetchMedia(Guid gid)
        {
            try
            {
                db = new SwpEntities();
                var media = db.Media_DataFiles.Where(p => p.PostId == gid).ToList().First();
                return media;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static ArrayList AddPost(Post post,HttpPostedFileBase pic,Int16 type)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                if(pic != null)
                {
                    var res = AddImage(pic);
                    if (Convert.ToBoolean(res[0]))
                    {
                        post.ProfilePicId = Guid.Parse(res[1].ToString());
                    }
                }
                post.Id = Guid.NewGuid();
                post.CreationDate = DateTime.Now;
                post.Type = Convert.ToByte(type);
                post.UserId = Guid.Parse("BC6DC859-F5AD-4288-BA72-113E5883A4E7"); //to configure
                db.Posts.Add(post);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add(post.Id);
                }
                  
                else
                {
                    submitResult.Add(false);
                }
            }
            catch (Exception)
            {

                submitResult.Add(false);
            }
            return submitResult;
        }

        public static ArrayList AddPaper(Paper_DataFiles pdf,Guid postId)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                pdf.Id = Guid.NewGuid();
                pdf.PostId = postId;
                db.Paper_DataFiles.Add(pdf);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("مقاله با موفقیت ثبت شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList AddMedia(Media_DataFiles mdf,Guid postId,bool type)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                mdf.Id = Guid.NewGuid();
                mdf.PostId = postId;
                mdf.UserId = Guid.Parse("BC6DC859-F5AD-4288-BA72-113E5883A4E7");
                if(File.Exists(mdf.FilePath))
                { 
                if(type) //film
                mdf.FilePath = Convert.ToString( "~" + "/" + "Files" + "/" + "Films" + "/" + mdf.FileName.ToString());
                else //voice
                    mdf.FilePath = Convert.ToString("~" + "/" + "Files" + "/" +  "Voices" + "/" + mdf.FileName.ToString());
                db.Media_DataFiles.Add(mdf);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("فایل شما با موفقیت ثبت شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.مجددا امتحان کنید");
                }
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("فایل بدرستی آپلود نشده است");
                }
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در دیتابیس.مجددا امتحان کنید");
            }
            return submitResult;
        }

        public static ArrayList SubmitDeletePost(Guid Pid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentPost = db.Posts.Where(p => p.Id == Pid).First();
                if (currentPost.Type == 3)
                {
                    if (currentPost.ProfilePicId != null)
                    {
                        var myimg1 = db.Img_DataFiles.Where(p => p.Id == currentPost.ProfilePicId).First();
                        db.Entry(myimg1).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        currentPost.ProfilePicId = null;
                        db.Entry(currentPost).Property("ProfilePicId").IsModified = true;
                        db.SaveChanges();
                    }
                    var paper = db.Paper_DataFiles.Where(p => p.PostId == currentPost.Id).ToList().First();
                    db.Entry(paper).State = System.Data.Entity.EntityState.Deleted;
                    if (db.SaveChanges() == 1)
                    {
                        db.Entry(currentPost).State = System.Data.Entity.EntityState.Deleted;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("پست با موفقیت حذف شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }
                else if (currentPost.Type == 2 || currentPost.Type == 1)
                {
                    if (currentPost.ProfilePicId != null)
                    {
                        var myimg = db.Img_DataFiles.Where(p => p.Id == currentPost.ProfilePicId).First();
                        db.Entry(myimg).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        currentPost.ProfilePicId = null;
                        db.Entry(currentPost).Property("ProfilePicId").IsModified = true;
                        db.SaveChanges();
                    }
                    var media = db.Media_DataFiles.Where(p => p.PostId == currentPost.Id).ToList().First();
                        db.Entry(media).State = System.Data.Entity.EntityState.Deleted;
                        if (db.SaveChanges() == 1)
                        {
                            db.Entry(currentPost).State = System.Data.Entity.EntityState.Deleted;
                            if (db.SaveChanges() == 1)
                            {
                                submitResult.Add(true);
                                submitResult.Add("پست با موفقیت حذف شد");
                            }
                            else
                            {
                                submitResult.Add(false);
                                submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                            }
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                }

                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitEditPost(EditPostViewModel current,HttpPostedFileBase pic)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                if (pic != null)
                {
                    var res = AddImage(pic);
                    if (Convert.ToBoolean(res[0]))
                    {
                        if(current.post.ProfilePicId != null)
                        { 
                        var oldpic = db.Img_DataFiles.Where(p => p.Id == current.post.ProfilePicId).First();
                        db.Entry(oldpic).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        }
                        current.post.ProfilePicId = Guid.Parse(res[1].ToString());
                    }
                }
                current.post.ModifiedDate = DateTime.Now;
                if(current.post.Type == 3)
                {
                    db.Entry(current.paper).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() == 1)
                    {
                        db.Entry(current.post).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("پست با موفقیت ویرایش شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }
                if(current.post.Type == 2 || current.post.Type == 1)
                {
                    db.Entry(current.media).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() == 1)
                    {
                        db.Entry(current.post).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("پست با موفقیت ویرایش شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }
                if (current.post.Type == 4)
                {
                    db.Entry(current.serat).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() == 1)
                    {
                        db.Entry(current.post).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("پست با موفقیت ویرایش شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
            }
            return submitResult;
        }

        //Book section

        public static IEnumerable<File_DataFiles> GetBooks()
        {
            try
            {
                db = new SwpEntities();
                return db.File_DataFiles.OrderBy(p => p.TimeStamp).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static File_DataFiles FetchBook(Guid guid)
        {
            try
            {
                db = new SwpEntities();
                return db.File_DataFiles.Where(p => p.Id == guid).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ArrayList AddBook(File_DataFiles book)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                book.Id = Guid.NewGuid();
                book.TimeStamp = DateTime.Now;
                book.UserId = Guid.Parse("BC6DC859-F5AD-4288-BA72-113E5883A4E7"); //to configure
                db.File_DataFiles.Add(book);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("کتاب با موفقیت ثبت شد");
                }

                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
            }
            catch (Exception)
            {

                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا به راهبر برنامه اطلاع دهید");
            }
            return submitResult;
        }

        public static ArrayList SubmitDeleteBook(Guid Pid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentBook = db.File_DataFiles.Where(p => p.Id == Pid).First();
                    db.Entry(currentBook).State = System.Data.Entity.EntityState.Deleted;
                    if (db.SaveChanges() == 1)
                    {
                    submitResult.Add(true);
                    submitResult.Add("کتاب با موفقیت حذف شد");
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                

                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitEditBook(File_DataFiles current)
        {
            submitResult = new ArrayList();
            try
            {
                db = new SwpEntities();
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("کتاب با موفقیت ویرایش شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
            }
            return submitResult;
        }

        //doubt section

        public static IEnumerable<Doubt> GetDoubts()
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(q => q.Type == true).OrderBy(p => p.TimeStamp).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Doubt FetchDoubt(Guid guid)
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(p => p.Id == guid).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList AddDoubt(Doubt doubt)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                doubt.Id = Guid.NewGuid();
                doubt.TimeStamp = DateTime.Now;
                doubt.IsExterior = false;
                doubt.Type = true;
                db.Doubts.Add(doubt);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("شبهه با موفقیت اضافه شد");
                }

                else
                {
                    submitResult.Add(false);
                }
                return submitResult;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static ArrayList SubmitDeleteDoubt(Guid Did)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentDoubt = db.Doubts.Where(p => p.Id == Did).First();
                            db.Entry(currentDoubt).State = System.Data.Entity.EntityState.Deleted;
                            if (db.SaveChanges() == 1)
                            {
                                submitResult.Add(true);
                                submitResult.Add("شبهه با موفقیت حذف شد");
                            }
                            else
                            {
                                submitResult.Add(false);
                                submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                            }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitEditDoubt(Doubt current)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() == 1)
                    {
                        submitResult.Add(true);
                        submitResult.Add("شبهه با موفقیت ویرایش شد");
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                
                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }

        //Question section

        public static IEnumerable<Doubt> GetQuestions()
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(q => q.Type == false).OrderBy(p => p.TimeStamp).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Doubt FetchQuestion(Guid guid)
        {
            try
            {
                db = new SwpEntities();
                return db.Doubts.Where(p => p.Id == guid).ToList().First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList AddQuestion(Doubt doubt)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                doubt.Id = Guid.NewGuid();
                doubt.TimeStamp = DateTime.Now;
                doubt.IsExterior = false;
                doubt.Type = false;
                db.Doubts.Add(doubt);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("سوال با موفقیت اضافه شد");
                }

                else
                {
                    submitResult.Add(false);
                }
                return submitResult;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static ArrayList SubmitDeleteQuestion(Guid Did)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentDoubt = db.Doubts.Where(p => p.Id == Did).First();
                db.Entry(currentDoubt).State = System.Data.Entity.EntityState.Deleted;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("سوال با موفقیت حذف شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitEditQuestion(Doubt current)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("سوال با موفقیت ویرایش شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }

                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }

        //slider section

        public static IEnumerable<LocalVariable> GetSliders()
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
        public static LocalVariable fetchSlider(Guid Lid)
        {
            try
            {
                db = new SwpEntities();
                var cat = db.LocalVariables.Where(p => p.Id == Lid).ToList().First();
                return cat;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ArrayList SubmitDeleteSlider(Guid Lid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentlocal = db.LocalVariables.Where(p => p.Id == Lid).First();
                db.Entry(currentlocal).State = System.Data.Entity.EntityState.Deleted;
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("اسلایدر با موفقیت حذف شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList SubmitAddSlider(LocalVariable current)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();                
                current.CreationDate = DateTime.Now;
                current.Id = Guid.NewGuid();
                db.LocalVariables.Add(current);
                if (db.SaveChanges() == 1)
                {
                    submitResult.Add(true);
                    submitResult.Add("اسلایدر با موفقیت اضافه شد");
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                }
                return submitResult;
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در برنامه.لطفا مجددا امتحان کنید");
                return submitResult;
            }
        }

        public static IEnumerable<tmp_PagesStatLogs> SimpleLogs()
        {
            try
            {
                db = new SwpEntities();
                return db.tmp_PagesStatLogs.OrderByDescending(p => p.count).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string[,] StatPagesBarChart()
        {
            string[,] data = new string[5,2];
            try
            {
                db = new SwpEntities();
                int i = 0;
                foreach (var item in db.tmp_PagesStatLogs.Take(1))
                {
                    data.SetValue(item.Url,i,0);
                    data.SetValue(item.count.ToString(),i,1);
                    i++;
                }
            }
            catch (Exception)
            {

            }
            return data;
        }

        public static string CloneBarChartData(string[,] input)
        {
            string output = "";
            try
            {
                output += "[";
                for (int i = 0; i < 1; i++)
                {
                    output += "{";
                    //output += "x:" + "^" + input[i, 0] + "^";
                    //output += ",";
                    output += "y:" + input[i, 1];
                    output += "}";
                    if(i < 0)
                        output += ",";
                }
                output += "]";
            }
            catch (Exception)
            {
            }
            return output;
        }

        public static ArrayList AddImage(HttpPostedFileBase source)
        {
            try
            {
                db = new SwpEntities();
                submitResult1 = new ArrayList();
                Img_DataFiles img = new Img_DataFiles() { CreationDate = DateTime.Now, FileExtention = "jpg", FileName = source.FileName, Size = source.ContentLength, UserId = Guid.Parse("BC6DC859-F5AD-4288-BA72-113E5883A4E7"), Id = Guid.NewGuid() };
                img.FileBody = new byte[source.ContentLength];
                source.InputStream.Read(img.FileBody, 0, source.ContentLength);
                db.Img_DataFiles.Add(img);
                if (db.SaveChanges() == 1)
                {
                    submitResult1.Add(true);
                    submitResult1.Add(img.Id);
                }
                else {
                    submitResult1.Add(false);
                }
                return submitResult1;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //serat section

        public static IEnumerable<Post> GetSerats()
        {
            try
            {
                db = new SwpEntities();
                return db.Posts.Where(q => q.Type == 4).OrderByDescending(p => p.CreationDate).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Media_DataFiles FetchSerat(Guid gid)
        {
            try
            {
                db = new SwpEntities();
                var serat = db.Media_DataFiles.Where(p => p.PostId == gid).ToList().First();
                return serat;
            }
            catch (Exception)
            {

                return null;
            }
        }


        public static ArrayList SubmitDeleteSerat(Guid Pid)
        {
            try
            {
                submitResult = new ArrayList();
                db = new SwpEntities();
                var currentPost = db.Posts.Where(p => p.Id == Pid).First();
                
                if (currentPost.Type == 4)
                {
                    if (currentPost.ProfilePicId != null)
                    {
                        var myimg = db.Img_DataFiles.Where(p => p.Id == currentPost.ProfilePicId).First();
                        db.Entry(myimg).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        currentPost.ProfilePicId = null;
                        db.Entry(currentPost).Property("ProfilePicId").IsModified = true;
                        db.SaveChanges();
                    }
                    var media = db.Media_DataFiles.Where(p => p.PostId == currentPost.Id).ToList().First();
                    db.Entry(media).State = System.Data.Entity.EntityState.Deleted;
                    if (db.SaveChanges() == 1)
                    {
                        db.Entry(currentPost).State = System.Data.Entity.EntityState.Deleted;
                        if (db.SaveChanges() == 1)
                        {
                            submitResult.Add(true);
                            submitResult.Add("پست با موفقیت حذف شد");
                        }
                        else
                        {
                            submitResult.Add(false);
                            submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                        }
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.لطفا مجددا امتحان کنید");
                    }
                }

                return submitResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ArrayList AddSerat(Media_DataFiles mdf, Guid postId, bool type)
        {
            try
            {
                db = new SwpEntities();
                submitResult = new ArrayList();
                mdf.Id = Guid.NewGuid();
                mdf.PostId = postId;
                mdf.UserId = Guid.Parse("BC6DC859-F5AD-4288-BA72-113E5883A4E7");
                if (File.Exists(mdf.FilePath))
                {
                    if (type) //film
                        mdf.FilePath = Convert.ToString("~" + "/" + "Files" + "/" + "Serat" + "/" + mdf.FileName.ToString());
                    db.Media_DataFiles.Add(mdf);
                    if (db.SaveChanges() == 1)
                    {
                        submitResult.Add(true);
                        submitResult.Add("فایل شما با موفقیت ثبت شد");
                    }
                    else
                    {
                        submitResult.Add(false);
                        submitResult.Add("مشکل در دیتابیس.مجددا امتحان کنید");
                    }
                }
                else
                {
                    submitResult.Add(false);
                    submitResult.Add("فایل بدرستی آپلود نشده است");
                }
            }
            catch (Exception)
            {
                submitResult.Add(false);
                submitResult.Add("مشکل در دیتابیس.مجددا امتحان کنید");
            }
            return submitResult;
        }

        public static string ToPersianCalender(DateTime georgian)
        {
            string txt = "";
            try
            {
                PersianCalendar pc = new PersianCalendar();
                txt += pc.GetSecond(georgian) + ":" + pc.GetMinute(georgian) + ":" + pc.GetHour(georgian) + " " +
                      pc.GetYear(georgian) + "/" + pc.GetMonth(georgian) + "/" + pc.GetDayOfMonth(georgian)    ;
            }
            catch (Exception)
            {
            }
            return txt;
        }

        public static byte[] FetchImage(string profilePicId)
        {
            try
            {
                db = new SwpEntities();
                Guid g = Guid.Parse(profilePicId);
                return db.Img_DataFiles.Where(p => p.Id == g).First().FileBody;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashkey));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashkey));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
    }
}