using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class Customer
    {
        [Key]
        public int CID { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactWebsite { get; set; }

    }
}
