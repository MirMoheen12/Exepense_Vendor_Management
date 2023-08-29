using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface ILogs
    {
        public void AddLog(string Event);
        public void ErrorLog(string Errormsg, string Desc);
        public List<Baselogs> GetAllLogs();
        public List<ErrorLogs> GetAllErrorLogs();
    }
}
