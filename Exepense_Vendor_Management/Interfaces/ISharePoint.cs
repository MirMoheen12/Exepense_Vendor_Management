namespace Exepense_Vendor_Management.Interfaces
{
    public interface ISharePoint
    {
        public Task<string> UploadToSharePointAsync(IFormFile file);
    }
}
