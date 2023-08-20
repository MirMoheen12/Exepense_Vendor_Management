using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using System;

namespace Exepense_Vendor_Management.Repositories
{
    public class CostExpenseRepo : ICostExp
    {
        private readonly IMedia media;
        private readonly AppDbContext appDbContext;
        public CostExpenseRepo(IMedia media,AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.media = media;

        }
        public bool AddNewCostExp(CostCenterExpense ce)
        {
            try
            {
                ce.isDeleted = false;
                ce.createdOn=DateTime.Now;
                ce.createdBy = "Mir";
                appDbContext.CostCenterExpense.Add(ce);
                appDbContext.SaveChanges();
                if (ce.SupportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ce.SupportingMedia;
                    m.mediaType = "Cost Center";
                   media.AddMedia(m,ce.id.ToString());
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
