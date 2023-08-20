
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exepense_Vendor_Management.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        public string mediaType { get; set; }
        public string fileName { get; set; }
        public string OldfileName { get; set; }

        public string? FileUrl { get; set; }
        public string ReqID { get; set; }
        [NotMapped]
        public IFormFile mediaFile { get; set; }
        public DateTime createdON { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }


    }
}
