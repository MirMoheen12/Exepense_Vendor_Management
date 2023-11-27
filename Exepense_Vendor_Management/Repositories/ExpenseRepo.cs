using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Repositories;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

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
        public decimal yearlylimt()
        {
            var data = appContext.Vendor.Where(x=>x.createdOn.Year==DateTime.Now.Year &&x.createdBy==user.ActiveUserId().Result).Sum(x => x.paymentAmount);
            return data.Value;
        }
        public decimal monthlylimt()
        {
            var data = appContext.Vendor.Where(x => x.createdOn.Month == DateTime.Now.Month && x.createdBy == user.ActiveUserId().Result).Sum(x => x.paymentAmount);
            return data.Value;
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
                    for (int i = 0; i < ex.SuportingMedia.Length; i++)
                    {


                        Media m = new Media();
                        m.mediaFile = ex.SuportingMedia[i];
                        m.mediaType = "Add Expense";
                        m.belongTo = "Expense";
                        m.belongcate = "Supporting Document";
                        media.AddMedia(m, ex.id.ToString());
                    }
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
        
        
        public async Task<bool> PostExpenseAsync(ExpenseData data)
        {
            HttpClient httpClient= new HttpClient();
            string apiUrl = "https://prod-07.eastus.logic.azure.com:443/workflows/a4e1091b78ff4a07b7521b9263c2c0ad/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=5xHG0g-PbIIDl4xGBD5iwtO5rKVjyv8ZHWLzBSU5Y0I";

            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
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

        public async Task<bool> ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile[]? file, float Amount)
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
                    data.ApprovedAmount= Convert.ToDecimal(Amount);
                }
                data.status = Fstatus;
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                appContext.EmployeeExpense.Update(data);
                appContext.SaveChanges();

                if (file != null)
                {
                    for (int i = 0; i < file.Length; i++)
                    {


                        Media m = new Media();
                        m.mediaFile = file[i];
                        m.mediaType = "Approve";
                        m.belongcate = "Approval Document";
                        m.belongTo = "Expense";
                        media.AddMedia(m, ID.ToString());
                    }
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
        public class ExpenseData
        {
            public string SubmittingUser { get; set; }
            public string Amount { get; set; }
            public string ExpenseCategory { get; set; }
            public string VendorName { get; set; }
            public string ExpenseDescription { get; set; }
        }
    }
}
