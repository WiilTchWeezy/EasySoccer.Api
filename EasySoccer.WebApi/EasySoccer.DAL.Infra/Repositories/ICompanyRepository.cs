using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAsync(string description, int page, int pageSize);
    }
}
