using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KitchenEquipmentMaintenance.Models
{
    public class Equipment
    {
        [Key]
        public int EquipmentId { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<RegisteredEquipment> RegisteredSites { get; set; }
        public int? FirstSiteId
        {
            get
            {
                return RegisteredSites?.FirstOrDefault()?.SiteId;
            }
        }
        public string FirstSiteDescription
        {
            get
            {
                var firstRegisteredSite = RegisteredSites?.FirstOrDefault();
                return firstRegisteredSite?.Site?.Description ?? "No Site";
            }
        }
    }

}
