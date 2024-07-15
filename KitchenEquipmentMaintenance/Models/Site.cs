using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenEquipmentMaintenance.Models
{
    public class Site
    {
        [Key]
        public int SiteId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<RegisteredEquipment> RegisteredEquipments { get; set; }
    }
}
