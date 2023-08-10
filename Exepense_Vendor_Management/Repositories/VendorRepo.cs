using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Repositories
{
    public class VendorRepo : IVendor
    {
        private readonly AppDbContext _context;
        private readonly IMedia media;
        public VendorRepo(AppDbContext _context, IMedia media)
        {
            this._context = _context;
            this.media = media;

        }
        public bool AddNewVendor(Vendor vendor)
        {
            try
            {
                vendor.createdOn = DateTime.Now;
                vendor.createdBy = "Mir";
                vendor.isDeleted = false;
                if (vendor.Contractdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.Contractdoc;
                    m.mediaType = "Add Vendor";
                    vendor.contractid = media.AddMedia(m);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
