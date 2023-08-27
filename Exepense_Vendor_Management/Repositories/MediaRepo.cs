using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Repositories
{
    public class MediaRepo : IMedia
    {
        private readonly AppDbContext _context;
        public MediaRepo(AppDbContext _context)
        {
            this._context = _context;
        }
        public int AddMedia(Media medias, string ReqID)
        {
          
                if(medias.mediaFile!=null)
                {
                    medias.fileName=Addfilesinserver(medias.mediaFile);
                    medias.isDeleted = false;
                    medias.createdBy = "Mir";
                    medias.ReqID=ReqID;
                    medias.createdON=DateTime.Now;
                    medias.OldfileName=medias.mediaFile.FileName;
                    medias.FileUrl = "azure url";
                    _context.Media.Add(medias);
                    _context.SaveChanges();
                return medias.Id;
                }
            return 0;
            
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
            var data = _context.Media.Where(x => x.Id == id && x.belongTo == belongTo && x.isDeleted == false ).ToList();
            return data;
        }
    }
}
