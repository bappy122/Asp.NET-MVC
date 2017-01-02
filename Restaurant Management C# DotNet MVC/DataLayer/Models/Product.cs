using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Enter Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Enter Category")]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Enter Price")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter valid Price")]
        public string ProductPrice { get; set; }
    }
}