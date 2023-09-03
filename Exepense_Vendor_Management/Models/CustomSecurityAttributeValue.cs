namespace Exepense_Vendor_Management.Models
{
    public class CustomSecurityAttributeValue
    {
        public string ODataType { get; set; }

        public string CostCenterODataType { get; set; }

        public List<string> CostCenter { get; set; }

        public string Department { get; set; }

        public string RolesForVendorAndExpenseMgt { get; set; }
    }
}
