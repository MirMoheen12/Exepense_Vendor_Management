using Exepense_Vendor_Management.Interfaces;
using Microsoft.SharePoint.Client;
using System.Net;
using PnP.Framework;
using AuthenticationManager = PnP.Framework.AuthenticationManager;

namespace Exepense_Vendor_Management.Repositories
{
    public class SharePointRepo : ISharePoint
    {
        private readonly IConfiguration _configuration;
        public SharePointRepo(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<string> UploadToSharePointAsync(IFormFile file, string mediaType)
        {
            string url= await Uploadfile(file, mediaType);
            return url;
        }
        private async Task<string> Uploadfile(IFormFile file, string mediaType)
        {
            var fileUrl = string.Empty;

            try
            {
                var siteUrl = _configuration.GetSection("SharePoint:SiteUrl").Value;
                var targetLibrary = string.Empty;
                switch (mediaType)
                {
                    case "Cost Center":
                        targetLibrary= _configuration.GetSection("SharePoint:CostCenterExpenseLibrary").Value;
                        break;
                    case "Add Vendor":
                        targetLibrary = _configuration.GetSection("SharePoint:VendorLibrary").Value;
                        break;
                    case "Add Expense":
                        targetLibrary = _configuration.GetSection("SharePoint:ExpenseLibrary").Value;
                        break;
                };

                var clientContext = await GetSharePointContext(siteUrl);

                Web web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQuery();

                var list = web.Lists.GetByTitle(targetLibrary);
                clientContext.Load(list.RootFolder);
                clientContext.ExecuteQuery();

                fileUrl = $"{list.RootFolder.ServerRelativeUrl}/{file.FileName}";

                // Convert IFormFile to Stream
                using (var stream = file.OpenReadStream())
                {
                    FileCreationInformation fileInfo = new FileCreationInformation
                    {
                        ContentStream = stream,
                        Url = file.FileName,
                        Overwrite = true
                    };

                    Microsoft.SharePoint.Client.File uploadFile = list.RootFolder.Files.Add(fileInfo);
                    clientContext.Load(uploadFile);
                    clientContext.ExecuteQuery();
                }
                return fileUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return fileUrl;

        }
        private async Task<ClientContext> GetSharePointContext(string siteUrl)
        {
            var cc = new AuthenticationManager().GetACSAppOnlyContext(siteUrl, _configuration.GetSection("SharePoint:ClientId").Value, _configuration.GetSection("SharePoint:ClientSecret").Value);
            return cc;
        }

    }
}
