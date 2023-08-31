using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Expense_Vendor_Management.Repositories
{
    public class ExpenseRepo : IExpense
    {
        private readonly IMedia media;
        private readonly AppDbContext appContext;
        private readonly IUser user;
        private readonly ILogs logs;

        public ExpenseRepo(IMedia media, AppDbContext appContext, IUser user, ILogs logs)
        {
            this.user = user;
            this.appContext = appContext;
            this.media = media;
            this.logs = logs;
        }

        public async Task<bool> AddNewExpense(EmployeeExpense ex)
        {
            try
            {
                ex.isDeleted = false;
                ex.createdOn = DateTime.Now;
                ex.createdBy = user.ActiveUserId();
                ex.modifiedBy = user.ActiveUserId();
                ex.status = "Submitted";
                appContext.EmployeeExpense.Add(ex);
                appContext.SaveChanges();

                if (ex.SuportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ex.SuportingMedia;
                    m.mediaType = "Add Expense";
                    m.belongTo = "Expense";
                    m.FileUrl = await SharePointClasses.UploadToSharePoint(ex.SuportingMedia);
                    media.AddMedia(m, ex.id.ToString());
                }

                logs.AddLog("AddNewExpense" + "New expense added.");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error adding new expense: {e.Message}", "AddNewExpense");
                return false;
            }
        }

        public List<EmployeeExpense> GetAllExpenses()
        {
            try
            {
                logs.AddLog("GetAllExpenses" + "Getting all expenses.");
                return appContext.EmployeeExpense.ToList();
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting all expenses: {e.Message}", "GetAllExpenses");
                return new List<EmployeeExpense>(); // Return an empty list or handle the error appropriately
            }
        }

        public EmployeeExpense GetExpById(int id)
        {
            try
            {
                logs.AddLog("GetExpById" + $"Getting expense with ID: {id}");
                return appContext.EmployeeExpense.FirstOrDefault(x => x.id == id);
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting expense by ID {id}: {e.Message}", "GetExpById");
                return null; // Handle the error appropriately
            }
        }

        public async Task<bool> ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            try
            {
                var data = appContext.EmployeeExpense.FirstOrDefault(x => x.id == ID);
                if (data == null)
                {
                    logs.AddLog("ChangeExpenseAction" + $"Expense with ID {ID} not found.");
                    return false;
                }

                data.status = Fstatus;
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                appContext.EmployeeExpense.Update(data);
                appContext.SaveChanges();

                if (file != null)
                {
                    Media m = new Media();
                    m.mediaFile = file;
                    m.mediaType = "Approve";
                    m.createdBy = "";
                    m.belongTo = "Expense";
                    m.FileUrl = await SharePointClasses.UploadToSharePoint(file);
                    media.AddMedia(m, ID.ToString());
                }

                logs.AddLog("ChangeExpenseAction" + $"Changed status for expense with ID: {ID}");
                return true;
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error changing expense action for ID {ID}: {e.Message}", "ChangeExpenseAction");
                return false;
            }
        }

        public void EditExpense(EmployeeExpense e)
        {
            try
            {
                appContext.EmployeeExpense.Update(e);
                appContext.SaveChanges();
                logs.AddLog("EditExpense" + $"Edited expense with ID: {e.id}");
            }
            catch (Exception ex)
            {
                logs.ErrorLog($"Error editing expense with ID {e.id}: {ex.Message}", "EditExpense");
            }
        }
    }
}
