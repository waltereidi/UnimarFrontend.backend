using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System.Security.Cryptography;
using UglyToad.PdfPig.Tokens;
using UnimarFrontend.backend.Configuration;

namespace UnimarFrontend.backend.GoogleDriveApi
{
    public class GoogleDriveRead : GoogleCredentialsSetup
    {
        private string _pageToken { get; set; }
        public GoogleDriveRead() 
        { 
            _pageToken = string.Empty;
        }
        private FilesResource.ListRequest GetConfiguredService()
        {
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = base.GetCredentials(),
                ApplicationName = "",
            });

            var request = service.Files.List();
            request.PageSize = 200;
            request.PageToken = _pageToken;
            return request;
        }

        //public IEnumerable<Google.Apis.Drive.v3.Data.File> GetDriveFiles()
        //{

        //    var service = new DriveService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = base.GetCredentials(),
        //        ApplicationName = "",
        //    });

        //    var request = service.Files.List();
        //    request.PageSize = 50;
        //    request.PageToken = _pageToken;

        //    var result = request.Execute();
        //    _pageToken = result.NextPageToken;
        //    return result.Files;
        //}
        private FileList? ExecuteRequest(FilesResource.ListRequest resource)
        {
            var result = resource.Execute();
            _pageToken = result.NextPageToken;
            return result;
        }
        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetDriveFiles()
        {
            var request = GetConfiguredService();
            SetFields(request);
            var result = ExecuteRequest(request);

            return result.Files;
        }
        private void SetFields(FilesResource.ListRequest request)
            => request.Fields = "files(id, name, mimeType, createdTime, modifiedTime, size, parents), nextPageToken";
            
        public IList<Google.Apis.Drive.v3.Data.File> GetDriveFilesFromCreationDate(DateTime? dataInicio = null , DateTime? dataFim = null )
        {
            if (dataInicio == null && dataFim == null)
                throw new ArgumentNullException();
                
                var request = GetConfiguredService();
            request.Q = $" trashed = false ";

            if (dataInicio != null)
            {
                string isoDate = dataInicio.Value
                   .ToUniversalTime()
                   .ToString("yyyy-MM-ddTHH:mm:ssZ");

                request.Q += $" and createdTime >= '{isoDate}' ";
            }

            if(dataInicio != null && dataFim != null )
                request.Q += $" and ";

            if (dataFim != null)
            {
                string isoDate = dataFim.Value
                   .ToUniversalTime()
                   .ToString("yyyy-MM-ddTHH:mm:ssZ");

                request.Q += $" createdTime <= '{isoDate}' ";
            }

            request.OrderBy = "createdTime";
            SetFields(request);
            request.PageToken = _pageToken;
            var fileList = ExecuteRequest(request);
            var result = fileList.Files;
            
            return result;

        }
    }
}