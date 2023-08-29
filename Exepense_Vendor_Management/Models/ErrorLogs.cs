using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class ErrorLogs
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Userid { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorDesc { get; set; }

    }
}
