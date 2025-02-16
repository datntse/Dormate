using FirebaseAdmin;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace Dormate.API.Services
{
    public interface IFirebaseService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task<List<string>> UploadMultipleFileAsync(IFormFile[] files, string folderName);
    }
    public class FirebaseSerivce : IFirebaseService
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;
        public FirebaseSerivce(IConfiguration configuration)
        {
            _bucketName = configuration["Firebase:Bucket"];
            var serviceAccountPath = configuration["Firebase:ServiceAccountPath"];

            if (FirebaseApp.DefaultInstance == null)
            {
                var urlServiceAccountPath = Path.Combine(Directory.GetCurrentDirectory(), serviceAccountPath);
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(urlServiceAccountPath)
                });
            }

            _storageClient = StorageClient.Create();
        }
        public async Task<string> UploadFileAsync(IFormFile file , string folderName)
        {
            var objectName = $"{folderName}/{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();
            var dataObject = await _storageClient.UploadObjectAsync(
                _bucketName, objectName, file.ContentType, stream);

            string publicUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
            return publicUrl;
        }

        public async Task<List<string>> UploadMultipleFileAsync(IFormFile[] files, string folderName)
        {
            var urls = new List<string>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var url = await UploadFileAsync(file, folderName);
                    urls.Add(url);
                }
            }
            return urls;
        }
    }
}
