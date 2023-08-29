using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Expense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Repositories
{
    public class LogRepo : ILogs
    {
        private readonly AppDbContext _context;
        private readonly IUser user;
        public LogRepo(AppDbContext _context,IUser user) 
        {
            this._context = _context;
            this.user = user;
        }
        public void AddLog(string Event)
        {
            Baselogs baselogs=new Baselogs();
            baselogs.CreatedBy = user.ActiveUserId();
            baselogs.CreatedOn=DateTime.Now;
            baselogs.Events = Event;       
            _context.Baselogs.Add(baselogs);
            _context.SaveChanges();
        }

        public void ErrorLog(string Errormsg,string Desc)
        {
            ErrorLogs errorLogs = new ErrorLogs();
            errorLogs.Userid = user.ActiveUserId();
            errorLogs.CreatedOn = DateTime.Now;
            errorLogs.ErrorMsg = Errormsg;
            errorLogs.ErrorDesc=Desc;
            _context.ErrorLogs.Add(errorLogs);
            _context.SaveChanges();
      
        }

        public List<ErrorLogs> GetAllErrorLogs()
        {
            return (_context.ErrorLogs.ToList());
        }

        public List<Baselogs> GetAllLogs()
        {
            return (_context.Baselogs.ToList());
        }
    }
}
