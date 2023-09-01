using Exepense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Repositories
{
    public class MediaRepo : IMedia
    {
        private readonly AppDbContext _context;
        private readonly IUser user;
        private readonly ILogs logs;
        private readonly ISharePoint sharePoint;

        public MediaRepo(AppDbContext _context, IUser user, ILogs logs, ISharePoint sharePoint)
        {
            this._context = _context;
            this.user = user;
            this.logs = logs;
            this.sharePoint = sharePoint;
        }

        public async Task<int> AddMedia(Media medias, string ReqID)
        {
            try
            {
                if (medias.mediaFile != null)
                {
                    medias.FileUrl = await Addfilesinserver(medias.mediaFile, medias.mediaType);
                    medias.fileName=medias.mediaFile.FileName;
                    medias.isDeleted = false;
                    medias.createdBy = user.ActiveUserId();
                    medias.ReqID = ReqID;
                    medias.createdON = DateTime.Now;
                    medias.OldfileName = medias.mediaFile.FileName;
                    if (string.IsNullOrEmpty(medias.FileUrl))
                    {
                        medias.FileUrl = medias.FileUrl;
                    }
                    else
                    {
                        medias.FileUrl = "AzureURL"; // You can update this logic
                    }
                    _context.Media.Add(medias);
                    _context.SaveChanges();

                    logs.AddLog("AddMedia" + "Media added.");
                    return medias.Id;
                }
                else
                {
                    logs.AddLog("AddMedia" + "Media file is null.");
                    return 0;
                }
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error adding media: {e.Message}", "AddMedia");
                return 0;
            }
        }

        public async Task<string> Addfilesinserver(IFormFile Files, string mediaType)
        {
            string fileName = await sharePoint.UploadToSharePointAsync(Files, mediaType);
         
            return fileName;
        }

        public List<Media> getAllMediaByID(int id, string belongTo)
        {
            try
            {
                var data = _context.Media.Where(x => x.ReqID == id.ToString() && x.belongTo == belongTo && x.isDeleted == false).ToList();
                logs.AddLog("getAllMediaByID" + "Retrieved media data.");
                return data;
            }
            catch (Exception e)
            {
                logs.ErrorLog($"Error getting media by ID {id}: {e.Message}", "getAllMediaByID");
                return new List<Media>();
            }
        }
    }
}
