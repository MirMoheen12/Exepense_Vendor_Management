using System.ComponentModel.DataAnnotations;

namespace Exepense_Vendor_Management.Models
{
    public class CostCenterExpense
    {
        [Key]
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public string modifiedBy { get; set; }
        public string isDeleted { get; set; }
        public string createdBy { get; set; }

        public DateTime submissionDate { get; set; }
        public float amount { get; set; }
        public DateTime expenseAccurred { get; set; }
        public string expenseCategory { get; set; }

        public string expenseDescription { get; set; }
        public string vandorName { get; set; }

        public string supportingDocs { get; set; }

        public string status { get; set; }

        public string notes { get; set; }


    }
}
