using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class UserAdmin
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}