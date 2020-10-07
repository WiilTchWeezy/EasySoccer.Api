using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IStateRepository: IRepositoryBase
    {
        Task<List<State>> GetAsync();
    }
}
