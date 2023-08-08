namespace Exepense_Vendor_Management.Models
{
    public class Vendor
    {
        public string vendorName { get; set; }
        public string status { get; set;}
        public string costCenter { get; set;}   
        public string poductType { get; set;}
        public string catagory { get; set;}
        public string criticalVendor { get; set;}
        public string contract { get; set;}
        public string assesments { get; set;}
        public string otherDocs { get;set;}
        public string paymentAmount { get; set;}
        public string autoPayment { get; set;}
        public  DateTime contractExpiration { get; set;}
        public string autoRenew{ get; set;}
        public DateTime dateToCancel { get; set; }

        public string notes{ get; set;}    

    }
}
