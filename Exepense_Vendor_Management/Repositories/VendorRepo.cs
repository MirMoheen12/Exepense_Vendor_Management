using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Repositories
{
    public class VendorRepo : IVendor
    {
        private readonly AppDbContext _context;
        private readonly IMedia media;
        private readonly IUser user;
        private readonly ILogs logs;

        public VendorRepo(AppDbContext _context, IMedia media, IUser user, ILogs logs)
        {
            this.user = user;
            this._context = _context;
            this.media = media;
            this.logs = logs;
        }

        public List<Vendor> GetActiveVendorsForms()
        {
            try
            {
                logs.AddLog("GetActiveVendorsForms" + "Getting active vendors.");
                return _context.Vendor.Where(x => x.isDeleted == false).ToList();
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting active vendors: {e.Message}", "GetActiveVendorsForms");
                return new List<Vendor>();
            }
        }

        public async Task<bool> AddNewVendor(Vendor vendor)
        {
            try
            {
                vendor.createdOn = DateTime.Now;
                vendor.createdBy = user.ActiveUserId().Result;
                vendor.modifiedBy = user.ActiveUserId().Result;
                vendor.notes = "Initial Insert";
                if (vendor.paymentAmount == null)
                {
                    vendor.paymentAmount = 0;
                }
                vendor.isDeleted = false;

                vendor.status = "On-Boarding";
                _context.Vendor.Add(vendor);
                await _context.SaveChangesAsync();

                if (vendor.Contractdoc != null || vendor.assesmentsdoc != null || vendor.otherdoc != null)
                {
                    if (vendor.Contractdoc != null)
                    {
                        Media m = new Media();
                        m.mediaFile = vendor.Contractdoc;
                        m.mediaType = "Add Vendor";
                        m.belongTo = "Vendor";
                        media.AddMedia(m, vendor.id.ToString());
                    }

                    if (vendor.assesmentsdoc != null)
                    {
                        Media m = new Media();
                        m.mediaFile = vendor.assesmentsdoc;
                        m.mediaType = "Add Vendor";
                        m.belongTo = "Vendor";
                      
                        media.AddMedia(m, vendor.id.ToString());
                    }

                    if (vendor.otherdoc != null)
                    {
                        Media m = new Media();
                        m.mediaFile = vendor.otherdoc;
                        m.mediaType = "Add Vendor";
                        m.createdBy = "";
                        m.belongTo = "Vendor";
                        media.AddMedia(m, vendor.id.ToString());
                    }
                }

                logs.AddLog("AddNewVendor" + "Added new vendor.");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error adding new vendor: {e.Message}", "AddNewVendor");
                return false;
            }
        }

        public Vendor GetVendorById(int vendorId)
        {
            try
            {
                logs.AddLog("GetVendorById" + $"Getting vendor with ID: {vendorId}");
                return _context.Vendor.FirstOrDefault(x => x.id == vendorId);
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting vendor by ID {vendorId}: {e.Message}", "GetVendorById");
                return null;
            }
        }

        public bool ChangeVendorAction(int ID, string Remarks, string Fstatus, IFormFile? file,string RNotfi)
        {
            try
            {
                var data = _context.Vendor.FirstOrDefault(x => x.id == ID);
                if (data == null)
                {
                    logs.AddLog("ChangeVendorAction" + $"Vendor with ID {ID} not found.");
                    return false;
                }

                data.status = Fstatus;
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                data.RNotfication= RNotfi;
                _context.Vendor.Update(data);
                _context.SaveChanges();

                if (file != null)
                {
                    Media m = new Media();
                    m.mediaFile = file;
                    m.mediaType = "Approve";
                    m.createdBy = "";
                    media.AddMedia(m, ID.ToString());
                }

                logs.AddLog("ChangeVendorAction" + $"Changed status for vendor with ID: {ID}");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error changing vendor action for ID {ID}: {e.Message}", "ChangeVendorAction");
                return false;
            }
        }

        public void EditVendor(Vendor v)
        {
            try
            {
                v.modifiedBy = user.ActiveUserId().Result;
                _context.Vendor.Update(v);
                _context.SaveChanges();
                logs.AddLog("EditVendor" + $"Edited vendor with ID: {v.id}");
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error editing vendor with ID {v.id}: {e.Message}", "EditVendor");
            }
        }
    }
}
