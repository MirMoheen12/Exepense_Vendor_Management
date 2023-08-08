namespace Exepense_Vendor_Management.Models
{
    public class EmployeeExpense
    {

        public DateTime submissionDate { get; set; }    
        public float amount { get; set; }
        public DateTime expenseAccurred { get; set; }
        public string expenseCategory { get; set; }

        public string vandorName { get;set; }

        public string supportingDocs { get; set; }

        public string status { get; set; }

        public string notes { get; set; }




    }
}
