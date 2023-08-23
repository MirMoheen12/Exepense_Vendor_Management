using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exepense_Vendor_Management.Models
{
    public class EmployeeExpense
    {
        [Key]
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public string? modifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }
        public DateTime submissionDate { get; set; }    
        public Nullable<float> amount { get; set; }
        public DateTime expenseOccurred { get; set; }
        public string expenseCategory { get; set; }

        public string vandorName { get;set; }
        [NotMapped]

        public IFormFile SuportingMedia { get; set; }
       
        public string status { get; set; } 

        public string notes { get; set; }




    }
}
