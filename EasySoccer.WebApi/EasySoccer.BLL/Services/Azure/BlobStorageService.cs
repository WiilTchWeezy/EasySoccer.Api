using EasySoccer.BLL.Infra.Services.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.Azure
{
    public class BlobStorageService : IBlobStorageService
    {
        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureCloudStorageConnection");
        }
        private string _connectionString = String.Empty;

        private CloudStorageAccount _storageAccount;
        private CloudStorageAccount storageAccount
        {
            get
            {
                if(_storageAccount == null)
                    _storageAccount = CloudStorageAccount.Parse(_connectionString);
                return _storageAccount;
            }
        }

        private CloudBlobClient _blobClient;
        private CloudBlobClient blobClient
        {
            get
            {
                if (_blobClient == null)
                    _blobClient = storageAccount.CreateCloudBlobClient();
                return _blobClient;
            }
        }

        public async void Delete(string fileName, string blobContainer)
        {
            var containerReference = blobClient.GetContainerReference(blobContainer);            
            var cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);
            await cloudBlockBlob.DeleteAsync();
        }

        public async Task<string> Save(byte[] bytes, string blobContainer, string fileName = "")
        {
            var containerReference = blobClient.GetContainerReference(blobContainer);
            await containerReference.CreateIfNotExistsAsync();
            if (string.IsNullOrEmpty(fileName))
                fileName = string.Format("{0}", Guid.NewGuid().ToString());
            var cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);
            using (var memoryStream = new MemoryStream(bytes))
            {
                await cloudBlockBlob.UploadFromStreamAsync(memoryStream);
            }
            return fileName;
        }
    }
}
