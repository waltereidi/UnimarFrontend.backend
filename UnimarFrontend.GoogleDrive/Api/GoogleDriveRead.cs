using UnimarFrontend.GoogleDrive.Configuration;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace UnimarFrontend.GoogleDrive.GoogleDrive.Api
{
    public class GoogleDriveRead : GoogleCredentialsSetup
    {
        private string _pageToken { get; set; }
        private int _defaultPageSize { get; set; }
        public GoogleDriveRead(int pageSize = 50)
        {
            _pageToken = string.Empty;
            _defaultPageSize = pageSize;
        }
        
        private FilesResource.ListRequest ConfigureListRequest()
        {
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = base.GetCredentials(),
                ApplicationName = "",
            });

            var request = service.Files.List();
            request.PageSize = _defaultPageSize;
            request.PageToken = _pageToken;
            return request;
        }
        private Google.Apis.Drive.v3.Data.FileList? ExecuteRequest(FilesResource.ListRequest request)
        {
            var result = request.Execute();
            _pageToken = result.NextPageToken;
            return result;
        }
        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetDriveFiles()
        {
            var request = ConfigureListRequest();
            var result = ExecuteRequest(request);
            return result.Files;
        }

        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetDriveFilesFromCreationDate(DateTime? dataInicio =null, DateTime? dataFim = null) 
        {
            if (dataInicio == null && dataFim == null)
                throw new ArgumentNullException();

            var request = ConfigureListRequest();
            request.Q += "trashed = false ";
            if (dataInicio != null)
            {
                var isoDate = dataInicio.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                request.Q += $" and createdTime >= '{isoDate}' ";
            }
            if(dataInicio != null && dataFim != null)
                request.Q += $" and ";

            if (dataFim != null)
            {
                var isoDate = dataFim.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                request.Q += $" createdTime <= '{isoDate}' ";
            }
            request.OrderBy = "createdTime";

            var result = ExecuteRequest(request);
            return result.Files;
        }

    }

}