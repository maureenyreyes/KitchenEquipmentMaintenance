namespace KitchenEquipmentMaintenance.Migrations
{
    using KitchenEquipmentMaintenance.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static KitchenEquipmentMaintenance.Models.Enumerations;

    internal sealed class Configuration : DbMigrationsConfiguration<KitchenEquipmentMaintenance.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "KitchenEquipmentMaintenance.Models.AppDbContext";
        }

        protected override void Seed(KitchenEquipmentMaintenance.Models.AppDbContext context)
        {
            context.Users.AddOrUpdate(
       u => u.UserName,
       new User
       {
           UserId = 1,
           UserType = UserType.SuperAdmin,
           FirstName = "Admin",
           LastName = "User",
           EmailAddress = "admin@example.com",
           UserName = "admin",
           Password = Helpers.PasswordHelper.HashPassword("admin123")
       }
   );
            context.SaveChanges();
        }
    }
}
