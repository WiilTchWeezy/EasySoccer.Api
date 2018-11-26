using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAsync(double? longitude, double? latitude, string description);
    }
}
