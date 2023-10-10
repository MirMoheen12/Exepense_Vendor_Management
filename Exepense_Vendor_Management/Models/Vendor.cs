using Exepense_Vendor_Management.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Vendor_Management.Models
{
    public class Vendor
    {

        [Key]
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public string modifiedBy { get; set; }
        public Boolean isDeleted { get; set; }
        public string createdBy { get; set; }
        public string vendorName { get; set; }
        public string status { get; set;}
        public string costCenter { get; set;}   
        public string poductType { get; set;}
        public string catagory { get; set;}
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactWebsite { get; set; }
        public string criticalVendor { get; set;}
        public string? renewalPeriod { get; set; }
        public string? RNotfication { get; set; }

        [NotMapped]
        public IFormFile? Contractdoc { get; set; } 
      
        [NotMapped]
        public IFormFile? assesmentsdoc { get; set; }
    
        [NotMapped]
        public IFormFile? otherdoc { get; set; }
        public decimal paymentAmount { get; set;}
        public string autoPayment { get; set;}
        public  DateTime contractExpiration { get; set;}
        public string autoRenew{ get; set;}
        public DateTime dateToCancel { get; set; }
        public string notes{ get; set;}  
        public Boolean TechnolgyVendor { get; set; }
        public Boolean CustomerAccess { get; set; }
   

    }
}
