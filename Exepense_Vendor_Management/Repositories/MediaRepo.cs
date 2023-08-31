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

        public MediaRepo(AppDbContext _context, IUser user, ILogs logs)
        {
            this._context = _context;
            this.user = user;
            this.logs = logs;
        }

        public int AddMedia(Media medias, string ReqID)
        {
            try
            {
                if (medias.mediaFile != null)
                {
                    medias.fileName = Addfilesinserver(medias.mediaFile);
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

        public string Addfilesinserver(IFormFile Files)
        {
            string fileName = Guid.NewGuid().ToString();
            string fileexten = Path.GetExtension(Files.FileName);
            fileName = fileName + fileexten;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UploadedFiles\", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Files.CopyTo(stream);
            }
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
