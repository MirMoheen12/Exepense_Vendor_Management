using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class EventLog
    {

        [Key]
        public int id { get; set; }
        public DateTime createdOn{get;set;}
        public string createdBy { get;set;}
        public string purpose { get;set;}

    }
}
