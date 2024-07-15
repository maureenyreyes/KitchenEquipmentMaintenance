using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KitchenEquipmentMaintenance.Models
{
    public class RegisteredEquipment
    {
        [Key]
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int SiteId { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Site Site { get; set; }
    }

}
