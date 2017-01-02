using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLayer.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Final_Project.Controllers
{
    public class BillingController : Controller
    {
        DataContext db = new DataContext();
        double subtotals = 0;
        double vat = 0;
        double totalPayable = 0;
        public ActionResult Index()
        {
            if (Session["UserType"] == null || Session["UserType"].Equals(" "))
            {
                return Redirect("/Start/Index");
            }
            ViewBag.billList = db.Bills.ToList();
            sumOfSubtotal();
            return View(db.Products.ToList());
        }

        [HttpPost]
        public ActionResult AddToBill(FormCollection collection)
        {
            if (Session["UserType"] == null || Session["UserType"].Equals(" "))
            {
                return Redirect("/Start/Index");
            }

            if (!productIdIsValid(collection["productId"]))
            {
                Response.Write("<script> alert('Product ID not Found..!'); </script>");
                
            }
            else
            {
                if (!quantityIsValid(collection["productQuantity"]))
                {
                    Response.Write("<script> alert('Quantity is Not Valid..!'); </script>");                   
                }
                else
                {
                    Product p = db.Products.Find(Convert.ToInt32(collection["productId"]));
                    Bill b = new Bill();
                    b.ProductName = p.ProductName;
                    b.Price = p.ProductPrice;
                    b.Quantity = collection["productQuantity"];
                    b.Subtotal =
                        Convert.ToString((Convert.ToDouble(p.ProductPrice))*
                                         (Convert.ToInt32(collection["productQuantity"])));
                    db.Bills.Add(b);
                    db.SaveChanges();
                    sumOfSubtotal();
                    ViewBag.billList = db.Bills.ToList();
                }
            }
            sumOfSubtotal();
            ViewBag.billList = db.Bills.ToList();
            return View(db.Products.ToList());
        }

        public ActionResult PrintBill()
        {
            if (Session["UserType"] == null || Session["UserType"].Equals(" "))
            {
                return Redirect("/Start/Index");
            }

            //Response.Write("<script> alert('PrintBill Called') <script>");
            sumOfSubtotal();
            makePdf(ViewBag.total.ToString(),ViewBag.vat.ToString(),ViewBag.payable.ToString());

            //add records To Sales Table
            {
                Sales s = new Sales();
                DateTime dateTime = DateTime.Today;

                foreach (var b in db.Bills.ToList())
                {
                    s.ProductName = b.ProductName;
                    s.Price = b.Price;
                    s.Quantity = b.Quantity;
                    s.Subtotal = b.Subtotal;
                    s.Date = dateTime.ToString("dd/MM/yyyy");
                    s.Time = DateTime.Now.ToString("hh:mm:ss tt");
                    s.Salesman = Session["Username"].ToString();
                    db.Saleses.Add(s);
                    db.SaveChanges();
                }
               
            }


            // clear bill table after printing the bill
            foreach (var data in db.Bills)
            {
                db.Bills.Remove(data);
            }
            db.SaveChanges();
            ViewBag.billList = db.Bills.ToList();
            return View(db.Products.ToList());
        }

        public ActionResult NewBill()
        {
            if (Session["UserType"] == null || Session["UserType"].Equals(" "))
            {
                return Redirect("/Start/Index");
            }

            foreach (var data in db.Bills)
            {
                db.Bills.Remove(data);
            }
            db.SaveChanges();
            sumOfSubtotal();
            ViewBag.billList = db.Bills.ToList();
            return View(db.Products.ToList());
        }

        public Boolean quantityIsValid(string quantity)
        {
            double qty;
            if (double.TryParse(quantity, out qty))
            {
                if (qty > 0)
                {
                    if (qty % 1 == 0)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean productIdIsValid(string id)
        {
            double qty;
            if (double.TryParse(id, out qty))
            {
                var user = db.Products.Find(Convert.ToInt32(id));
                if (user != null)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["UserType"] == null || Session["UserType"].Equals(" "))
            {
                return Redirect("/Start/Index");
            }

            Bill b = db.Bills.Find(id);
            db.Bills.Remove(b);
            db.SaveChanges();
            ViewBag.billList = db.Bills.ToList();
            sumOfSubtotal();

            return RedirectToAction("Index");
        }

        public void sumOfSubtotal()
        {
            double subtotals = 0;
            double vat = 0;
            double totalPayable = 0;
            foreach (var b in db.Bills.ToList())
            {
                subtotals += Convert.ToDouble(b.Subtotal);
            }
            vat = subtotals*.15;
            totalPayable = vat + subtotals;

            ViewBag.vat = vat;
            ViewBag.total = subtotals;
            ViewBag.payable = totalPayable;
        }

        public void makePdf(string total,string vat,string payable)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                            new DataColumn("Product Name", typeof(string)),
                            new DataColumn("Price", typeof(string)),
                            new DataColumn("Quantity", typeof(string)),
                            new DataColumn("Subtotal", typeof(string))});

            {
                foreach (var p in db.Bills)
                {
                    string name = p.ProductName;
                    string price = p.Price;
                    string qty = p.Quantity;
                    string subtotal = p.Subtotal;

                    dt.Rows.Add(name, price, qty, subtotal);
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Generate Invoice (Bill) Header.
                    sb.Append("<table width='100%' cellspacing='0' align='center' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>ABC Restaurant</td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Branch: Dhanmondi, Road: 15, House: 5, D-Block</td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Phone: 0212382, Email: abcrestaurant@gmail.com</td></tr>");

                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td></br><b>Salesman :</b>  " + Session["Username"] + " </b>");
                    //sb.Append(orderNo);
                    sb.Append("</td><td align = 'right'><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");

                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Products and Details</b></td></tr>");

                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<br />");

                    //Generate Invoice (Bill) Items Grid.
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Product Name");
                    sb.Append("</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Product Price");
                    sb.Append("</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Product Quantity");
                    sb.Append("</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Subtotal");
                    sb.Append("</th>");

                    sb.Append("</tr>");
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<td align='center'>");
                            sb.Append(row[column]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Total</td>");
                    sb.Append("<td align = 'center'>");
                    sb.Append(total);
                    sb.Append("</td> </tr>");

                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Vat</td>");
                    sb.Append("<td align = 'center'>");
                    sb.Append(vat);
                    sb.Append("</td> </tr>");

                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Payable</td>");
                    sb.Append("<td align = 'center'>");
                    sb.Append(payable);
                    sb.Append("</td> </tr>");

                    sb.Append("</table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=bill.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    //Response.End();
                }
            }
        }

        public void clearBillTable()
        {
            foreach (var data in db.Bills)
            {
                db.Bills.Remove(data);
            }
            db.SaveChanges();
            sumOfSubtotal();
            ViewBag.billList = db.Bills.ToList();
        }
    }
}
