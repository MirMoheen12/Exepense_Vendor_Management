using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

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
                int count = appContext.EmployeeExpense.ToList().Count() + 1;
                ex.createdBy = user.ActiveUserId().Result;
                var name = user.GetUserName(ex.createdBy).Result.ToString();
                if (name != null && name != "NotFound")
                {
                   var newid = name.Split(' ');
                    ex.Vid = newid.LastOrDefault() + "-" + count.ToString("D4");
                }
                else
                {
                    ex.Vid = "NotFound-" + count;
                }
                
              
                ex.isDeleted = false;
                ex.createdOn = DateTime.Now;
                ex.modifiedBy = user.ActiveUserId().Result;
                ex.notes = "Intial insert";
                ex.status = "Submitted";
                appContext.EmployeeExpense.Add(ex);
                appContext.SaveChanges();

                if (ex.SuportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ex.SuportingMedia;
                    m.mediaType = "Add Expense";
                    m.belongTo = "Expense";
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
        string getlastename(string name)
        {
            var res = name.Split(" ");
            if(res.Length > 1 ) {
                return res[1];
            }
            return "Notfound";
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
               // logs.AddLog("GetExpById" + $"Getting expense with ID: {id}");
                return appContext.EmployeeExpense.FirstOrDefault(x => x.id == id);
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting expense by ID {id}: {e.Message}", "GetExpById");
                return null; // Handle the error appropriately
            }
        }

        public async Task<bool> ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file, float Amount)
        {
            try
            {
                var data = appContext.EmployeeExpense.FirstOrDefault(x => x.id == ID);
                if (data == null)
                {
                    logs.AddLog("ChangeExpenseAction" + $"Expense with ID {ID} not found.");
                    return false;
                }
                if (Amount == null)
                {
                    data.ApprovedAmount = 0;
                }
                else
                {
                    data.ApprovedAmount= Amount;
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
                    m.belongTo = "Expense";
                  
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
