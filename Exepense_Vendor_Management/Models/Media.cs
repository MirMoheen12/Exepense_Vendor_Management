
using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        public string mediaType { get; set; }
        public string fileName { get; set; }
        public DateTime createdON { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }


    }
}
