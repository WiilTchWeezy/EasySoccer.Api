using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISportTypeRepository
    {
        Task<List<SportType>> GetAsync(long companyId);
    }
}
