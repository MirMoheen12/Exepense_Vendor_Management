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
                vendor.contractid = -1;
                vendor.assesmentsid = -1;
                vendor.otherDocsid = -1;
                if (vendor.Contractdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.Contractdoc;
                    m.mediaType = "Add Vendor";
                    vendor.contractid = media.AddMedia(m);

                }
                if (vendor.assesmentsdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.assesmentsdoc;
                    m.mediaType = "Add Vendor";
                    vendor.assesmentsid = media.AddMedia(m);

                }
                if (vendor.otherdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.otherdoc;
                    m.mediaType = "Add Vendor";
                    vendor.otherDocsid = media.AddMedia(m);

                }
                vendor.status = "On-Boarding";
                _context.Vendor.Add(vendor);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
