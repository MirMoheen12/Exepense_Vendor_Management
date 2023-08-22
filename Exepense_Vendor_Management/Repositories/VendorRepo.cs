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
        public List<Vendor> GetActiveVendorsForms() 
        {
            return (_context.Vendor.Where(x => x.isDeleted == false).ToList());
        
        }
        public bool AddNewVendor(Vendor vendor)
        {
            try
            {
                vendor.createdOn = DateTime.Now;
                vendor.createdBy = "Mir";
                vendor.modifiedBy = "Mir";
                vendor.isDeleted = false;

                vendor.status = "On-Boarding";
                _context.Vendor.Add(vendor);
                _context.SaveChanges();
                if (vendor.Contractdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.Contractdoc;
                    m.mediaType = "Add Vendor";
                    media.AddMedia(m,vendor.id.ToString());

                }
                if (vendor.assesmentsdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.assesmentsdoc;
                    m.mediaType = "Add Vendor";
                    media.AddMedia(m, vendor.id.ToString());

                }
                if (vendor.otherdoc != null)
                {
                    Media m = new Media();
                    m.mediaFile = vendor.otherdoc;
                    m.mediaType = "Add Vendor";
                    m.createdBy = "";
                    media.AddMedia(m, vendor.id.ToString());

                }
        
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
