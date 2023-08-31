using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Expense_Vendor_Management.Repositories
{
    public class CostExpenseRepo : ICostExp
    {
        private readonly ILogs logs;
        private readonly IMedia media;
        private readonly IUser user;
        private readonly AppDbContext appDbContext;

        public CostExpenseRepo(IMedia media, AppDbContext appDbContext, IUser user, ILogs logs)
        {
            this.appDbContext = appDbContext;
            this.media = media;
            this.user = user;
            this.logs = logs;
        }

        public async Task<bool> AddNewCostExp(CostCenterExpense ce)
        {
            try
            {
                ce.isDeleted = false;
                ce.createdOn = DateTime.Now;
                ce.createdBy = user.ActiveUserId();
                ce.modifiedBy = user.ActiveUserId();
                ce.status = "Submitted";
                appDbContext.CostCenterExpense.Add(ce);
                appDbContext.SaveChanges();

                if (ce.SupportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ce.SupportingMedia;
                    m.mediaType = "Cost Center";
                    m.belongTo = "Cost";
                    m.FileUrl = await SharePointClasses.UploadToSharePoint(ce.SupportingMedia);
                    media.AddMedia(m, ce.id.ToString());
                }

                logs.AddLog("AddNewCostExp" + "New cost expense added.");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog(e.Message, "AddNewCostExp");
                return false;
            }
        }

        public List<CostCenterExpense> GetAllCost()
        {
            logs.AddLog("GetAllCost" + "Getting all cost center expenses.");
            return appDbContext.CostCenterExpense.Where(x => x.isDeleted == false).ToList();
        }

        public CostCenterExpense GetCostById(int vendorId)
        {
            try
            {
                logs.AddLog("GetCostById" + $"Getting cost center expense with ID: {vendorId}");
                return appDbContext.CostCenterExpense.FirstOrDefault(x => x.id == vendorId);
            }
            catch (Exception)
            {
                logs.ErrorLog("Error getting cost center expense.", "GetCostById");
                return null;
            }
        }

        public async Task<bool> ChangeCostAction(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            try
            {
                var data = appDbContext.CostCenterExpense.FirstOrDefault(x => x.id == ID);
                data.status = Fstatus;
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                appDbContext.CostCenterExpense.Update(data);
                appDbContext.SaveChanges();

                if (file != null)
                {
                    Media m = new Media();
                    m.mediaFile = file;
                    m.mediaType = "Approve";
                    m.createdBy = user.ActiveUserId();
                    m.belongTo = "Cost";
                    m.FileUrl = await SharePointClasses.UploadToSharePoint(file);
                    media.AddMedia(m, ID.ToString());
                }

                logs.AddLog("ChangeCostAction" + $"Changed status for cost center expense with ID: {ID}");
                return true;
            }
            catch (Exception)
            {
                logs.ErrorLog("Error changing cost center expense action.", "ChangeCostAction");
                return false;
            }
        }

        public void EditCostExp(CostCenterExpense ce)
        {
            appDbContext.CostCenterExpense.Update(ce);
            appDbContext.SaveChanges();
            logs.AddLog("EditCostExp" + $"Edited cost center expense with ID: {ce.id}");
        }
    }
}
