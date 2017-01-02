using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DataLayer.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Final_Project.Controllers
{
    public class SalesController : Controller
    {
        DataContext db = new DataContext();
        static int flag = 0;
        private static string searchKey ="";
        
        public ActionResult Index()
        {
            //Sessionc Check
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }
            ViewBag.PrintBill = db.Saleses.ToList();
            return View(db.Saleses.ToList());
        }

        public ActionResult Search(FormCollection collection)
        {
            
            //Sessionc Check
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }

            if (collection["searchKey"].Equals("") || collection["searchKey"] == null)
            {
                Response.Write("<script> alert('Enter Text to Search..!') </script>");
            }
            else
            {
                string s = collection["searchKey"];
                searchKey = "";
                searchKey += s;
                ViewBag.list = db.Saleses.Where(a => a.Date.Contains(s) || a.Price.Contains(s) || a.ProductName.Contains(s) || a.Quantity.Contains(s) || a.Salesman.Contains(s) || a.Subtotal.Contains(s) || a.Time.Contains(s)).ToList();
                ViewBag.PrintBill = ViewBag.list;
                flag = 1;
                return View(ViewBag.list);
            }
   

            return View(db.Saleses.ToList());
        }

        public ActionResult Refresh()
        {
           return Redirect("/Sales/Index");
        }
        public ActionResult GenerateReport(FormCollection collection)
        {
            if (!Session["UserType"].Equals("admin") && Session["UserType"] != null)
            {
                return Redirect("/Start/Index");
            }

            if (flag == 0)
                return View(db.Saleses.ToList());
            else
                flag = 0;

            ViewBag.list = db.Saleses.Where(a => a.Date.Contains(searchKey) || a.Price.Contains(searchKey) || a.ProductName.Contains(searchKey) || a.Quantity.Contains(searchKey) || a.Salesman.Contains(searchKey) || a.Subtotal.Contains(searchKey) || a.Time.Contains(searchKey)).ToList();
            
            double total = 0;
            foreach (var p in ViewBag.list)
            {
                total += Convert.ToDouble(p.Subtotal);
            }

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] {
                            new DataColumn("Product", typeof(string)),
                            new DataColumn("Quantity", typeof(string)),
                            new DataColumn("Price", typeof(string)),
                            new DataColumn("Subtotal", typeof(string)),
                            new DataColumn("Date", typeof(string)),
                            new DataColumn("Time", typeof(string)),
                            new DataColumn("Salesman", typeof(string))});

            {
                //fetching data from cuustom query
                foreach (var p in ViewBag.list)
                {
                    string name = p.ProductName;
                    string price = p.Price;
                    string qty = p.Quantity;
                    string subtotal = p.Subtotal;
                    string date = p.Date;
                    string time = p.Time;
                    string salesman = p.Subtotal;

                    dt.Rows.Add(name, qty, price, subtotal, date, time, salesman);
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
                    sb.Append("<tr><td></br><b>Generated By :</b>  " + Session["Username"] + " </b>");
                    //sb.Append(orderNo);
                    sb.Append("</td><td align = 'right'><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");

                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Sales Report [ Search Filter : " + searchKey+"]</b></td></tr>");

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
                    sb.Append("Product Quantity");
                    sb.Append("</th>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Product Price");
                    sb.Append("</th>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Subtotal");
                    sb.Append("</th>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Date");
                    sb.Append("</th>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Time");
                    sb.Append("</th>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#000000' align='center'>");
                    sb.Append("Salesman");
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
                    sb.Append(Convert.ToString(total));
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


            return View(db.Saleses.ToList());
        }
    }
}
