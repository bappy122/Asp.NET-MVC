using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

         [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    if (EmailAddressExists(user.Email, user))
                    {
                        Response.Write("<script>alert('Your Email Address is Already Registered...!')</script>");
                    }
                    else
                    {
                        //check if the user already requested for the account
                        var requested = db.Users.Where(a => a.Email.Equals(user.Email)).FirstOrDefault();
                        if (requested == null)
                        {
                            user.AccountStatus = "Deactive";
                            db.Users.Add(user);
                            db.SaveChanges();
                            Response.Write("<script>alert('Your Account Request has been forwarded to Admin for Aproval.')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('You have Aready a Request Pending.')</script>");
                        }
                    }
                  

                }
            }
             return View();
        }

        public Boolean EmailAddressExists(string email,User user)
        {
            using (DataContext db = new DataContext())
            {
                var exists = db.LoginTables.Where(a=> a.Email.Equals(user.Email)).FirstOrDefault();
                if(exists != null)
                {
                    return true;
                }
                return false;
            }            
        }

    }
}
