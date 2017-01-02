using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class SalesmanLoginController : Controller
    {
        DataContext db = new DataContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginTable user)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var obj = db.LoginTables.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password) && a.AccountStatus.Equals("Active")).FirstOrDefault();
                    
                    if (obj != null)
                    {
                        Session["Username"] = user.Username;
                        Session["UserType"] = "salesman";
                       // Response.Write("<script>alert('Username && Password Matched...!'')</script>");
                       // Response.Write("<script>alert('"+Session["Username"]+ Session["UserId"]+"')</script>");
                        return Redirect("/Billing/Index");
                    }
                    else
                    {
                        var accountDeactive = db.LoginTables.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password) && a.AccountStatus.Equals("Deactive")).FirstOrDefault();
                        //chek if the account is deactive     
                        if (accountDeactive != null)
                        {
                            Response.Write("<script>alert('Your Account is Deactive..!')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Username && Password Did not Matched..!')</script>");
                        }
                        
                    }
                }
            }
            return View("Login");
        }
    }
}
