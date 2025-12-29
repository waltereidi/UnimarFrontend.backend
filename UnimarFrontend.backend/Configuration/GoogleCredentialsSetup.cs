using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnimarFrontend.backend.Configuration
{
    public abstract class GoogleCredentialsSetup 
    {
        private GoogleCredential _credentials;
        private DirectoryInfo _projectDir;
        private readonly string _configFileName = "credentials.json";
        public GoogleCredentialsSetup()
        {
            Console.WriteLine($"GoogleCredentialsSetup {Directory.GetParent(AppContext.BaseDirectory)}");
            Console.WriteLine($"GoogleCredentialsSetup {Directory.GetParent(AppContext.BaseDirectory)
                            .Parent.Parent.Parent.Parent.FullName}");
            _projectDir = Directory.GetParent(AppContext.BaseDirectory);

#if DEBUG
_projectDir = new
                (
                    Path.Combine(
                        Directory.GetParent(AppContext.BaseDirectory)
                            .Parent.Parent.Parent.Parent.FullName, "UnimarFrontend.GoogleDriveApi")
                );


            string credentialPath = Path.Combine("Configuration", _configFileName);
            if (File.Exists(credentialPath))
            {
                using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
                {
                    _credentials = GoogleCredential.FromStream(stream)
                        .CreateScoped(DriveService.ScopeConstants.Drive);
                }
            }
#endif
            using (var stream = new FileStream(Path.Combine(_projectDir.FullName ,"Configuration",_configFileName), FileMode.Open, FileAccess.Read))
            {
                _credentials = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.ScopeConstants.Drive);
            }
            
        }
        public GoogleCredential GetCredentials() => _credentials;

    }
}
