
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Routing.Constraints;
using UnimarFrontend.backend.Configuration;
using UnimarFrontend.backend.Interfaces;

namespace UnimarFrontend.backend.GoogleDriveApi
{
    public class GoogleDriveDownload : GoogleCredentialsSetup , IGoogleDriveDownloadStrategy
    {
        public record DownloadRequest(string fileId , string fileName , DirectoryInfo output);
        private DownloadRequest _request;
        public GoogleDriveDownload(DownloadRequest request){ 
            _request = request;
        }
        public GoogleDriveDownload(Google.Apis.Drive.v3.Data.File file , DirectoryInfo output)
        {
            _request = new DownloadRequest(file.Id , file.Name , output );
        }
        public GoogleDriveDownload(string fileId , string fileName , string output )
        {
            _request = new DownloadRequest(fileId , fileName , new DirectoryInfo(output) );
        }

        public FileInfo Start()
        {
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = base.GetCredentials(),
                ApplicationName = "",
            });

            var request = service.Files.Get(_request.fileId);

            using (var memoryStream = new MemoryStream())
            {
                // Faz o download para o stream
                request.Download(memoryStream);
                var filePath = Path.Combine(_request.output.FullName, _request.fileName );
                // Salva no caminho especificado
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fileStream);
                    return new FileInfo(filePath);
                }
            }
        }
    }
}
