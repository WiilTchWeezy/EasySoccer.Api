using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.Azure
{
    public interface IBlobStorageService
    {
        Task<string> Save(byte[] bytes, string blobContainer, string fileName = "");
        void Delete(string fileName, string blobContainer);
    }
}
