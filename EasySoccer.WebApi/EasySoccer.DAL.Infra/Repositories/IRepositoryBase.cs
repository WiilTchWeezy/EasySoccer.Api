using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface IRepositoryBase
    {
        Task Edit<T>(T entity) where T : class;
        Task Create<T>(T entity) where T : class;
        Task Delete<T>(T entity) where T : class;
    }
}
