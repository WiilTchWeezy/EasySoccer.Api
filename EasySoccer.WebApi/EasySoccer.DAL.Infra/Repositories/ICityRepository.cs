﻿using EasySoccer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ICityRepository : IRepositoryBase
    {
        Task<City> GetAsync(int Id);

        Task<List<City>> GetCitiesByState(int idState);
    }
}
