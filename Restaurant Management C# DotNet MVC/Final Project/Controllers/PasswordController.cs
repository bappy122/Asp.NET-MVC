using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class PasswordController : Controller
    {
        //
        // GET: /Password/

        public ActionResult Index()
        {
            //Session Check
            if (Session["UserType"].ToString().Equals(" ") || Session["Username"] == null)
            {
                return Redirect("/Start/Index");
            }
            if (Session["UserType"].ToString().Equals("admin"))
            {
                return Redirect("/Password/Change"); 
            }
            if (Session["UserType"].ToString().Equals("salesman"))
            {
                return Redirect("/Password/Change"); 
            }
            return Redirect("/Password/Index");
        }

        public ActionResult Change()
        {
            if (Session["UserType"].ToString().Equals(" ") || Session["UserType"] == null)
            {
                return Redirect("/Start/Index");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Change(ChangePassword change)
        {
            if (Session["UserType"].ToString().Equals(" ") || Session["UserType"] == null)
            {
                return Redirect("/Start/Index");
            }

            if (ModelState.IsValid)
            {
                switch (success(change))
                {
                    case 1:
                        Response.Write("<script>alert('Password Successfuly Changed...!')</script>");
                        break;
                    case 2:
                        Response.Write("<script>alert('Current Password Did not Match...!')</script>");
                        break;
                }
            }
            return View("Change");
        }

        public int success(ChangePassword change)
        {
            using (DataContext db = new DataContext())
            {
                string id = "";

                // Salesman
                if (Session["UserType"].ToString().Equals("salesman"))
                {
                    foreach (var p in db.LoginTables)
                    {
                        if (p.Password.Equals(change.CurrentPassword) && p.Username.Equals(Session["Username"].ToString()))
                        {
                            id += p.id;
                            break;
                        }
                    }
                    if (!id.Equals(""))
                    {
                        LoginTable user = db.LoginTables.Find(Convert.ToInt32(id));
                        user.Password = change.NewPassword;
                        db.SaveChanges();
                        return 1;
                    }

                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    // admin
                    foreach (var p in db.UserAdmins)
                    {
                        if (p.Password.Equals(change.CurrentPassword) && p.Username.Equals(Session["Username"].ToString()))
                        {
                            id += p.id;
                            break;
                        }
                    }
                    if (!id.Equals(""))
                    {
                        UserAdmin user = db.UserAdmins.Find(Convert.ToInt32(id));
                        user.Password = change.NewPassword;
                        db.SaveChanges();
                        return 1;
                    }

                    else
                    {
                        return 2;
                    }
                }
            }
        }

    }
}
