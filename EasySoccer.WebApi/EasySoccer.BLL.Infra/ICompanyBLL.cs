using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyBLL
    {
        Task<List<Company>> GetAsync(double? longitude, double? latitude, string description, int page, int pageSize);
    }
}
