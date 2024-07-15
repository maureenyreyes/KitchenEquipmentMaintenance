using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KitchenEquipmentMaintenance.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<RegisteredEquipment> RegisteredEquipments { get; set; }
        public AppDbContext() : base("name=AppDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
            Database.Initialize(false); // Initialize the database only if it doesn't exist
        }
    }
}
