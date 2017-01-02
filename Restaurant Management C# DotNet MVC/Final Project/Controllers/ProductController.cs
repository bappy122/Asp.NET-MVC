using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Models;

namespace Final_Project.Controllers
{
    public class ProductController : Controller
    {
        DataContext db = new DataContext();
        //
        // GET: /Product/

        public ActionResult Index()
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.Products.ToList());
        }

        public ActionResult Details(int id)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.Products.Find(id));
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
                using (db)
                {
                    if (ModelState.IsValid)
                    {
                        db.Products.Add(p);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(p);
                    }
                }


                
            
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.Products.Find(id));
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            try
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            return View(db.Products.Find(id));
        }

        //
        // POST: /Product/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Product p)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }

            try
            {
                Response.Write("alert('inside delete')");
                //Product p = new Product();
                p = db.Products.Find(id);
                db.Products.Remove(p);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
