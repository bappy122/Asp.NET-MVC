using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final_Project.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            //Sessionc Check
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View();
        }

    }
}
