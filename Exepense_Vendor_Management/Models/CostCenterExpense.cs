using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exepense_Vendor_Management.Models
{
    public class CostCenterExpense
    {
        [Key]
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public string modifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }

        public DateTime submissionDate { get; set; }
        public Nullable<float> amount { get; set; }
        public DateTime expenseAccurred { get; set; }
        public string expenseCategory { get; set; }

        public string expenseDescription { get; set; }
        public string vandorName { get; set; }
        [NotMapped]
        public IFormFile SupportingMedia { get; set; }
  
        public string status { get; set; }

        public string notes { get; set; }


    }
}
