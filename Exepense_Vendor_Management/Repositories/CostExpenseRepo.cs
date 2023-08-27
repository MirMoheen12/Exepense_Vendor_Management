using Exepense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Expense_Vendor_Management.Repositories
{
    public class CostExpenseRepo : ICostExp
    {
        private readonly IMedia media;
        private readonly IUser user;
        private readonly AppDbContext appDbContext;
        public CostExpenseRepo(IMedia media,AppDbContext appDbContext, IUser user)
        {
            this.appDbContext = appDbContext;
            this.media = media;
            this.user = user;   
        }
        public bool AddNewCostExp(CostCenterExpense ce)
        {
            try
            {
                ce.isDeleted = false;
                ce.createdOn=DateTime.Now;
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
                    media.AddMedia(m,ce.id.ToString());
                }
             
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<CostCenterExpense> GetAllCost()
        {
            return (appDbContext.CostCenterExpense.Where(x => x.isDeleted == false).ToList());

        }

        public CostCenterExpense GetCostById(int vendorId)
        {
            try
            {
                var dat = appDbContext.CostCenterExpense.Where(x => x.id == vendorId).FirstOrDefault();
                return dat;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool ChangeCostAction(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            try
            {
                var data = appDbContext.CostCenterExpense.Where(x => x.id == ID).FirstOrDefault();
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
                    m.createdBy = "";
                    m.belongTo = "Cost";
                    media.AddMedia(m, ID.ToString());
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EditCostExp(CostCenterExpense ce)
        {
            appDbContext.CostCenterExpense.Update(ce);
            appDbContext.SaveChanges();
        }
    }
}
