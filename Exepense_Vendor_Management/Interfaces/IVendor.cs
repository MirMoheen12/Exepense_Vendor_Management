using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface IVendor
    {
        public List<Vendor> GetActiveVendorsForms();
        public bool AddNewVendor(Vendor vendor);
        public Vendor GetVendorById(int vendorId);
        public bool ChangeVendorAction(int ID, string Remarks, string Fstatus, IFormFile? file);
    }
}
