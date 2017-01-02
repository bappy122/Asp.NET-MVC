using System.Data.Entity.Migrations;
using DataLayer.Models;

namespace DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DataLayer.Models.DataContext";
        }

        protected override void Seed(DataContext context)
        {
            var admin = new UserAdmin();
            admin.Username = "admin";
            admin.Password = "admin";
            context.UserAdmins.Add(admin);
            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}