using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Z11.Models;
using System.IO;
namespace Z11.Controllers
{
    public class HomeController : Controller
    {
        private MyDb1 db = new MyDb1();
        /// <summary>
        /// 
        /// </summary>
        public  HomeController()
        {
            /*
            var route = RouteData.Values.ToArray();
            if (route[1].ToString() == "login")
            {


            }else
            {
                if (System.Web.HttpContext.Current.Session["userId"] == null || System.Web.HttpContext.Current.Session["userId"].ToString() == "")
                {
                    System.Web.HttpContext.Current.Response.Redirect("/Home/Login");
                }
            }
            */
        }
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["userId"] == null || System.Web.HttpContext.Current.Session["userId"].ToString() == "")
            {
                return RedirectToAction("Login");
            }
            IQueryable<UserInfo> userinfoes =  db.UserInfo.Where(q => q.id > 0);
            if (Request.IsAjaxRequest())
            {
                return PartialView(userinfoes);
            }
            return View(userinfoes);
        }

        public ActionResult MyInfo()
        {
            ViewBag.Message = "Your application description page.";
            int sid = int.Parse(Session["userId"].ToString());
            UserInfo userinfo = db.UserInfo.Where(q => q.userId == sid).First();
            return View(userinfo);
        }
        public ActionResult MyInfo1()
        {
            ViewBag.Message = "Your application description page.";
			try{
				int sid = int.Parse(Session["userId"].ToString());
				IQueryable<Texts> textinfoes = db.Texts.Where(q => q.userId == sid);
				UserInfo userinfo = db.UserInfo.Where(q => q.userId == sid).First();
				uInfoandText u = new uInfoandText();
				u.userinfo = userinfo;
				u.texts = textinfoes;
				u.mycount = textinfoes.Count();
				u.allcount = db.Texts.Count();
				if (Request.IsAjaxRequest())
				{
					return PartialView(u);
				}
				return View(u);
			}catch
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult PersonInfo(String id)
        {
            int sid = int.Parse(id);
            IQueryable<UserInfo> userinfo = db.UserInfo.Where(q => q.userId == sid);
            UserInfo uf = userinfo.First();
            IQueryable<Texts> textinfoes = db.Texts.Where(q => q.userId == sid);
            uInfoandText u = new uInfoandText();
            u.userinfo = uf;
            u.texts = textinfoes;
            u.allcount = db.Texts.Count();
            u.mycount = textinfoes.Count();
            if (Request.IsAjaxRequest())
            {
                return PartialView(u);
            }
            return View(u);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }
        public ActionResult Login()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }
        public ActionResult getLoginInfo()
        {
            String username = Request.Form["username"].ToString();
            String pwd = Request.Form["password"].ToString();
            IQueryable<Users> user = db.Users.Where(q=>q.username==username&&q.password==pwd);
            int flag = 0;
            foreach(var v in user)
            {
                flag++;
                Session["userId"] = v.Id;
                Session["username"] = v.username;
            }
            
            if (flag==1)
            {

                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Rsuc()
        {
            return View("registerSuccessful");
        }
        public ActionResult getRegisterInfo()
        {
            String username = Request.Form["username"].ToString();
            String pwd = Request.Form["password"].ToString();
            String rpwd = Request.Form["rpassword"].ToString();
            String email = Request.Form["email"].ToString();
            String url = Url.Action("Rsuc", "Home");
            Users user = new Users();
            user.username = username;
            user.email = email;
            user.password = pwd;
            if(user.username != null && user.password != null && user.username != "" && user.password != "")
            {
                if (pwd == rpwd)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    Users user2 = db.Users.Where(q => q.username == username && q.password == pwd).First();
                    int uid = user2.Id;
                    UserInfo uf = new UserInfo();
                    uf.name = username;
                    uf.userId = uid;
                    uf.email = email;
                    uf.skill = "暂无";
                    uf.skillvalue = "0";
                    uf.introduction = "本人暂无简介";
                    uf.imgUrl = "/files/imgs/li.png";
                    db.UserInfo.Add(uf);
                    db.SaveChanges();

                    return Redirect(url);
                }
                return View("Login", user);
            }else
            {
                return RedirectToAction("Login");
            }
           
        }
        public ActionResult EditInfo(String id)
        {
            int sid = int.Parse(Request["selectid"]);
            IQueryable<UserInfo> userinfo = db.UserInfo.Where(q=>q.userId==sid);
            UserInfo uf=userinfo.First();
           
            return View(id,uf);
        }
        public ActionResult EditInfo1(String id)
        {
            int sid = int.Parse(Request["selectid"]);
            IQueryable<UserInfo> userinfo = db.UserInfo.Where(q => q.userId == sid);
            UserInfo uf = userinfo.First();

            if (Request.IsAjaxRequest())
            {
                return PartialView(id, uf);
            }
            return View(id, uf);
        }
        
        public ActionResult EditText(String id)
        {
            int sid = int.Parse(id);
            IQueryable<Texts> textinfo = db.Texts.Where(q => q.Id == sid);
            Texts tf = textinfo.First();
            if (Request.IsAjaxRequest())
            {
                return PartialView(tf);
            }
            return View(tf);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            //获取图片文件名
            var fileName = System.IO.Path.GetFileName(upload.FileName);
            var filePhysicalPath = Server.MapPath("~/files/imgs/" + fileName);//我把它保存在网站根目录的 upload 文件夹，需要在项目中添加对应的文件夹
            upload.SaveAs(filePhysicalPath);  //上传图片到指定文件夹
            var url = "/files/imgs/" + fileName;
            var CKEditorFuncNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];
            //上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个tab选项
            return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        }

        [ValidateInput(false)]
        public ActionResult EditTextSubmit(String id)
        {
            int sid = int.Parse(id);
            String title = Request["title"];
            String content = Request["content"];
            Texts tf = new Texts();
            tf.Content = content;
            tf.Title = title;
            db.SaveChanges();
            return RedirectToAction("MyInfo1");

        }
       
        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditSubmit(HttpPostedFileBase file)
        {
            var f = Request.Files["fileup"];
            string url = "";
            int flag = 0;
            if (f.ContentLength > 0)
            {
                flag = 1;
                url = Server.MapPath("~/files/imgs/") + Path.GetFileName(f.FileName);
                f.SaveAs(url);
                url = "/files/imgs/" + Path.GetFileName(f.FileName);


            }
            String s = Request["ssid"];
            try
            {
                int userid = int.Parse(Session["userId"].ToString());
                String name = Request["name"];
                String job = Request["job"];
                String email = Request["email"];
                String phone = Request["phone"];
                String skill = Request["skill"];
                //String skillvalue = Request["skillvalue"];
                String skillvalue = 0 + "";
                String introduction = Request["introduction"];
                String back = Request["back"];
                String address = Request["address"];
                String workExperience = Request["workExperience"];
                UserInfo uf = db.UserInfo.SingleOrDefault(d => d.userId == userid);
                uf.name = name;
                uf.job = job;
                uf.email = email;
                uf.telPhone = phone;
                uf.address = address;
                uf.skill = skill;
                uf.skillvalue = skillvalue;
                uf.introduction = introduction;
                uf.background = back;
                uf.workExperience = workExperience;
                if (flag == 1)
                {
                    uf.imgUrl = url;
                }
                db.SaveChanges();
                return RedirectToAction("MyInfo1");
            }
            catch
            {
                return RedirectToAction("Login");
            }
            
            
        }

        public ActionResult NewText(String id)
        {
            uId um = new uId();
			try
            {
				int sid = int.Parse(Session["userId"].ToString());
				um.id = sid;
				um.content = "";
				if (Request.IsAjaxRequest())
				{
					return PartialView(um);
				}
				else
				{
					return View(um);
				}
			}catch
            {
                return RedirectToAction("Login");
            }
        }

        [ValidateInput(false)]
        public ActionResult NewTextSubmit(String id)
        {
			try
            {
				int sid = int.Parse(Session["userId"].ToString());
				String title = Request["title"];
				String content = Request["content"];
				Texts tf = new Texts();
				tf.Content = content;
				tf.Title = title;
				tf.userId = sid;
				db.Texts.Add(tf);
				db.SaveChanges();
				return RedirectToAction("MyInfo1");
			}catch
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult DeleteText(String id)
        {
            uId u = new uId();
            int sid = int.Parse(id);
            u.id = sid;         
            try
            {
                Texts t = db.Texts.SingleOrDefault(q => q.Id == sid);
                db.Texts.Remove(t);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                   return RedirectToAction("MyInfo1");
                }
                else
                {
                    return RedirectToAction("MyInfo1");
                }
            }
            catch
            {
                u.id = -1;
                if (Request.IsAjaxRequest())
                {
                    return PartialView();
                }
                else
                {
                    return PartialView();
                }
            }
        }
       

        public ActionResult ChangePwd()
        {
            String oldpwd = Request["old"];
            String newpwd = Request["newp"];
            String rnewpwd = Request["rnew"];
            String username = Session["username"].ToString();
            IQueryable<Users> u = db.Users.Where(q => q.username == username && q.password == oldpwd);
            int flag = 0;
            foreach (var v in u)
            {
                flag = 1;
            }
            var route = RouteData.Values.ToArray();
            if (flag == 0)
            {
                
                //Response.Write("<script>alert('旧密码错误，请重新输入！')</script>");
                return Json(0);
            }else if (newpwd != rnewpwd)
            {
                return Json(1);
                    //JavaScript("alert('新密码前后不一致！')");
            }
            else
            {
                Users us = db.Users.SingleOrDefault(d => d.username == username&&d.password==oldpwd);
                us.password = rnewpwd;
                db.SaveChanges();
                Session.RemoveAll();
            }
            return Json(2);
        }

        public ActionResult LoginOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
        public ActionResult PreMain()
        {
            IQueryable<Texts> textsinfoes = db.Texts.Where(q => q.Id > 0);
            if (Request.IsAjaxRequest())
            {
                return PartialView(textsinfoes);
            }
            return View(textsinfoes);
        }
        public ActionResult PreList(String id)
        {
            Texts tf;
            try
            {
                int sid = int.Parse(Request["selectid"]);
                IQueryable<Texts> textinfo = db.Texts.Where(q => q.Id == sid);
                tf = textinfo.First();
            }
            catch
            {
                IQueryable<Texts> textinfo = db.Texts.Where(q => q.Id == 1);
                tf = textinfo.First();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(id,tf);
            }
            return View(id,tf);
        }
    }
}