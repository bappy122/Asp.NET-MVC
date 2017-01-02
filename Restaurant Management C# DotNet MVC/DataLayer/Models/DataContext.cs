using System.Data.Entity;

namespace DataLayer.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAdmin> UserAdmins { get; set; }
        public DbSet<LoginTable> LoginTables { get; set; }
        public DbSet<ChangePassword> ChangePasswords { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Sales> Saleses { get; set; }
    }
}