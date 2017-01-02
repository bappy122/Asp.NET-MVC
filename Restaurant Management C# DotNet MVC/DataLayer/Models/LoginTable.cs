using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class LoginTable
    {
        [Key]
        public int id { get; set; }

        [ScaffoldColumn(false)]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
         [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [ScaffoldColumn(false)]
        public string UserType { get; set; }
        [ScaffoldColumn(false)]
        public string AccountStatus { get; set; }
    }
}