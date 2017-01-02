using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class AccountsController : Controller
    {
        DataContext db = new DataContext();
       
        public ActionResult Index()
        {
            //Sessionc Check
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.LoginTables.ToList());
        }

        public ActionResult Activate(int id)
        {
            using (db)
            {
                LoginTable user = new LoginTable();
                user = db.LoginTables.Find(id);

                if(user.AccountStatus.Equals("Active"))
                {
                    Response.Write("<script>alert('Account is Already Active...!')</script>");
                }
                else
                {
                    user.AccountStatus = "Active";
                    
                }

                db.SaveChanges();
            }
            return Redirect("../Index");
        }

        public ActionResult Deactivate(int id)
        {
            using (db)
            {
                LoginTable user = new LoginTable();
                user = db.LoginTables.Find(id);
                if (user.AccountStatus.Equals("Deactive"))
                {
                    Response.Write("<script>alert('Account is Already Deactive...!')</script>"); 
                }
                else
                {
                    user.AccountStatus = "Deactive";
                    Response.Write("<script>alert('Account is Deactivated...!')</script>");           
                }
                db.SaveChanges();
            }
            return Redirect("../Index");
        }

        public ActionResult Delete(int id)
        {
            using (db)
            {
                LoginTable user = new LoginTable();
                user = db.LoginTables.Find(id);
                db.LoginTables.Remove(user);
                db.SaveChanges();
            }
            return Redirect("../Index");
        }


    }
}
