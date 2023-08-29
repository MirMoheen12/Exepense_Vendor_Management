using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class Baselogs
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Events { get; set; }
    }
}
