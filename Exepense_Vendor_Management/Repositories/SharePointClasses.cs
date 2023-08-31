using Microsoft.SharePoint.Client;
using System.Net;
using PnP.Framework;
using AuthenticationManager = PnP.Framework.AuthenticationManager;

namespace Exepense_Vendor_Management.Repositories
{
    public class SharePointClasses
    {
        private static IConfiguration configuration23;

        public static async Task<string> UploadToSharePoint(IFormFile file)
        {
            var fileUrl= string.Empty;

            try
            {
                var siteUrl = "https://rizemtg.sharepoint.com/sites/AccountingInt";
                var targetLibrary = "VendorApprovalDocumentation";
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
       private static async Task<ClientContext> GetSharePointContext(string siteUrl)
        {
           
            //var cc = new AuthenticationManager().GetACSAppOnlyContext(siteUrl, clientID, clientSecret);
            var cc = new AuthenticationManager().GetACSAppOnlyContext(siteUrl, configuration23.GetSection("SharePoint:ClientId").Value, configuration23.GetSection("SharePoint:ClientSecret").Value);
            return cc;
        }


    }
}
