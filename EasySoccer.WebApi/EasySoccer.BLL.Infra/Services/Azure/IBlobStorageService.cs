using EasySoccer.BLL.Infra.Services.Azure.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.Azure
{
    public interface IBlobStorageService
    {
        Task<string> Save(byte[] bytes, BlobContainerEnum blobContainer, string fileName = "");
        void Delete(string fileName, BlobContainerEnum blobContainer);
    }
}
