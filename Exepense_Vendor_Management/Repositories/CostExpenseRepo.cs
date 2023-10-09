using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Expense_Vendor_Management.Repositories
{
    public class CostExpenseRepo : ICostExp
    {
        private readonly ILogs logs;
        private readonly IMedia media;
        private readonly IUser user;
        private readonly AppDbContext appDbContext;
        public CostExpenseRepo(IMedia media,AppDbContext appDbContext, IUser user,ILogs logs)
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
                var count = appDbContext.CostCenterExpense.ToList().Count()+1;
                ce.isDeleted = false;
                ce.displayid = ce.costcenterid.Substring(ce.costcenterid.Length - 4)+" -"+count;
                ce.createdOn=DateTime.Now;
                ce.createdBy = user.ActiveUserId().Result;
                ce.modifiedBy = user.ActiveUserId().Result;
                ce.status = "Submitted";
                appDbContext.CostCenterExpense.Add(ce);
                appDbContext.SaveChanges();
                if (ce.SupportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ce.SupportingMedia;
                    m.mediaType = "Cost Center";
                    m.belongTo = "Cost";
                    media.AddMedia(m,ce.id.ToString());
                }
                logs.AddLog("AddNewCostExp");
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
            logs.AddLog("GetAllCost");
            return (appDbContext.CostCenterExpense.Where(x => x.isDeleted == false).ToList());

        }

        public CostCenterExpense GetCostById(int vendorId)
        {
            try
            {
               // logs.AddLog("GetCostById");
                return appDbContext.CostCenterExpense.Where(x => x.id == vendorId).FirstOrDefault();
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ChangeCostAction(int ID, string Remarks, string Fstatus, IFormFile? file,float Amount)
        {
            try
            {
                var data = appDbContext.CostCenterExpense.Where(x => x.id == ID).FirstOrDefault();
                data.status = Fstatus;
                if (Amount != null)
                {
                    data.ApprovedAmount = Amount.ToString();
                }
                else
                {
                    data.ApprovedAmount = "0";
                }
             
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                appDbContext.CostCenterExpense.Update(data);
                appDbContext.SaveChanges();
                if (file != null)
                {
                    Media m = new Media();
                    m.mediaFile = file;
                    m.mediaType = "Approve";
                    m.createdBy = user.ActiveUserId().Result;
                    m.belongTo = "Cost";
                    media.AddMedia(m, ID.ToString());
                }
                logs.AddLog("ChangeCostAction");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog(e.Message, "ChangeCostAction");
                return false;
            }
        }

        public void EditCostExp(CostCenterExpense ce)
        {
            try
            {
                logs.AddLog("EditCostExp");
                appDbContext.CostCenterExpense.Update(ce);
                appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                logs.ErrorLog(e.Message, "EditCostExp");
                throw;
            }
        
        }
    }
}
