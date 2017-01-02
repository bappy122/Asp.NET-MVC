using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class AccountRequestController : Controller
    {
        DataContext db = new DataContext();
        public ActionResult Index()
        {           
            //Sessionc Check
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.Users.ToList());
        }

        public ActionResult Accept(int id,User user)
        {
            using (db)
            {
                LoginTable newUser = new LoginTable();
                user = db.Users.Find(id);

                newUser.Email = user.Email;
                newUser.Username = user.Username;
                newUser.Password = user.Password;
                newUser.UserType = "Salesman";
                newUser.AccountStatus = "Active";
                db.LoginTables.Add(newUser);

                db.Users.Remove(user);
                db.SaveChanges();
            }
            return Redirect("../Index");
        }

        public ActionResult Reject(int id, User user)
        {
            using (db)
            {
                user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return Redirect("../Index");
        }

    }
}
