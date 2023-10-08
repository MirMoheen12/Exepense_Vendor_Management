using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class CostCenter
    {
        [Key]
        public int ID { get; set; }
        public string CostCenterID {get; set;}
        public string Userid { get; set;}
    }
}
