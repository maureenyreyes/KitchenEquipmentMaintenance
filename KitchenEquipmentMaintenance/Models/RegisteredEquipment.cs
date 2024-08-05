using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenEquipmentMaintenance.Models
{
    public class RegisteredEquipment
    {
        [Key]
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int? SiteId { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual Equipment Equipment { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }

}
