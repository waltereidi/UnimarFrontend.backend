using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using UnimarFrontend.backend.GoogleDriveApi.Configuration;


namespace UnimarFrontend.backend.GoogleDriveApi.GoogleDrive
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
            request.PageSize = 50;
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

            var result = ExecuteRequest(request);

            return result.Files;
        }

        public object GetDriveFilesFromCreationDate(DateTime? dataInicio, DateTime? dataFim)
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

            var result = ExecuteRequest(request);

            return result.Files;

        }
    }
}