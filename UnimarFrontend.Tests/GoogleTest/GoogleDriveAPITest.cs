using Framework.Tests;
using FrameworkGoogleApi.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnimarFrontend.backend.GoogleDriveApi.GoogleDrive;

namespace UnimarFrontend.Tests.GoogleTest
{
    public class GoogleDriveAPITest : Configuration
    {
        private readonly GoogleDriveRead _serviceRead;
        public GoogleDriveAPITest() : base("UnimarFrontEnd.GoogleDriveApi", "Configuration")
        {
            _serviceRead = new GoogleDriveRead();
        }
        [Fact]
        public void TesteListar()
        {
            var result = _serviceRead.GetDriveFilesFromCreationDate(DateTime.Now.AddDays(-20) , DateTime.Now);
            var result2 = _serviceRead.GetDriveFiles();
            var result3 = _serviceRead.GetDriveFiles();
            var result4 = _serviceRead.GetDriveFiles();

            Assert.NotNull(result2);
        }
        [Fact]
        public void TestDownload()
        {

            var result = _serviceRead.GetDriveFiles();
            var file = result.First();

            IGoogleDriveDownloadStrategy downloadService = new GoogleDriveDownload(file, base._fileOutputDir);


            var fi = downloadService.Start();
            Assert.True(fi.Exists);
        }

    }
}
