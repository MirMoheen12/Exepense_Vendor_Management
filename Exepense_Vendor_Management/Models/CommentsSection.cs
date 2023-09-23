using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class CommentsSection
    {
        [Key]
        public int CSID { get; set; }
        public string Description { get; set; }
        public string Commentsby { get; set; }
        public int ID { get; set; }
        public string Comfor { get; set; }
        public DateTime CommentsDate { get; set; }
    }
}
