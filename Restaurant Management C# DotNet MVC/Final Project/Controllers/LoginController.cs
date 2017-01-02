using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class LoginController : Controller
    {
        DataContext db =new DataContext();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserAdmin user)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var obj = db.UserAdmins.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Username"] = user.Username;
                        Session["UserType"] = "admin";
                        Response.Write("<script>alert('Username && Password Matched!')</script>");
                        return Redirect("/Admin/Index");
                    }
                    else
                    {
                        Response.Write("<script>alert('Username && Password Dont Match...!')</script>");
                    }
                }
                return View("Login");
            }
            else
            {
                return View(user);
            }
            //return View("Login");
        }
    }
}
