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
        public string criticalVendor { get; set;}
     
        [NotMapped]
        public IFormFile? Contractdoc { get; set; } 
      
        [NotMapped]
        public IFormFile? assesmentsdoc { get; set; }
    
        [NotMapped]
        public IFormFile? otherdoc { get; set; }
        public string paymentAmount { get; set;}
        public string autoPayment { get; set;}
        public  DateTime contractExpiration { get; set;}
        public string autoRenew{ get; set;}
        public DateTime dateToCancel { get; set; }
        public string notes{ get; set;}    

    }
}
