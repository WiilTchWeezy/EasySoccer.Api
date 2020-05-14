using EasySoccer.BLL.Infra.Services.Azure;
using EasySoccer.BLL.Infra.Services.Azure.Enums;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.Azure
{
    public class BlobStorageService : IBlobStorageService
    {

        private string _connectionString = ConfigurationManager.ConnectionStrings["AzureCloudStorageConnection"].ConnectionString;

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

        public async void Delete(string fileName, BlobContainerEnum blobContainer)
        {
            var containerReference = blobClient.GetContainerReference(nameof(blobContainer));            
            var cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);
            await cloudBlockBlob.DeleteAsync();
        }

        public async Task<string> Save(byte[] bytes, BlobContainerEnum blobContainer, string fileName = "")
        {
            var containerReference = blobClient.GetContainerReference(nameof(blobContainer));
            await containerReference.CreateIfNotExistsAsync();
            if (string.IsNullOrEmpty(fileName))
                fileName = string.Format("{0}.png", Guid.NewGuid().ToString());
            var cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);
            using (var memoryStream = new MemoryStream(bytes))
            {
                await cloudBlockBlob.UploadFromStreamAsync(memoryStream);
            }
            return fileName;
        }
    }
}
