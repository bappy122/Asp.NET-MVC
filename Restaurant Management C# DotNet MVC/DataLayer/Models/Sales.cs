using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class Sales
    {
        [Key]
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Subtotal { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Salesman { get;set; }

    }
    
}