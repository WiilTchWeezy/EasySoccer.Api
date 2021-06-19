using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Model;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Repositories
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        public CompanyRepository(IEasySoccerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<CompanyModel>> GetAsync(int page, int pageSize, string name, string orderField, string orderDirection, double? longitude, double? latitude)
        {
            var query = _dbContext.CompanyQuery.Include(x => x.City).Where(x => x.Active == true && x.Location != null).Skip((page - 1) * pageSize).Take(pageSize);
            if (string.IsNullOrEmpty(name) == false)
                query = query.Where(x => x.Name.Contains(name));
            if (orderField == "Name" && orderDirection == "ASC")
                query = query.OrderBy(x => x.Name);
            if (orderField == "Name" && orderDirection == "DESC")
                query = query.OrderByDescending(x => x.Name);
            if (longitude.HasValue == false || latitude.HasValue == false)
                query = query.OrderBy(x => x.Name);
            var companies = new List<CompanyModel>();
            if (string.IsNullOrEmpty(orderField) || orderField == "Location")
            {
                if (longitude.HasValue && latitude.HasValue)
                {
                    var currentLocation = new Point(longitude.Value, latitude.Value) { SRID = 4326 };
                    if (orderDirection == "DESC")
                        query = query.OrderByDescending(x => x.Location.Distance(currentLocation));
                    else
                        query = query.OrderBy(x => x.Location.Distance(currentLocation));
                    companies = await query.Select(x => new CompanyModel
                        {
                            Active = x.Active,
                            CityName = x.City.Name,
                            Name = x.Name,
                            CNPJ = x.CNPJ,
                            CompanySchedules = x.CompanySchedules.ToList(),
                            CompleteAddress = x.CompleteAddress,
                            CreatedDate = x.CreatedDate,
                            Description = x.Description,
                            Distance = x.Location.Distance(currentLocation),
                            IdCity = x.IdCity,
                            Latitude = (double)x.Latitude,
                            Logo = x.Logo,
                            Longitude = (double)x.Longitude,
                            WorkOnHoliDays = x.WorkOnHoliDays
                        }).ToListAsync();
                }
                else
                {
                    companies = await query.Select(x => new CompanyModel
                    {
                        Active = x.Active,
                        CityName = x.City.Name,
                        Name = x.Name,
                        CNPJ = x.CNPJ,
                        CompanySchedules = x.CompanySchedules.ToList(),
                        CompleteAddress = x.CompleteAddress,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Distance = -1,
                        Id = x.Id,
                        IdCity = x.IdCity,
                        Latitude = (double)x.Latitude,
                        Logo = x.Logo,
                        Longitude = (double)x.Longitude,
                        WorkOnHoliDays = x.WorkOnHoliDays

                    }).ToListAsync();
                }
            }
            else
            {
                companies = await query.Select(x => new CompanyModel
                {
                    Active = x.Active,
                    CityName = x.City.Name,
                    Name = x.Name,
                    CNPJ = x.CNPJ,
                    CompanySchedules = x.CompanySchedules.ToList(),
                    CompleteAddress = x.CompleteAddress,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    Distance = -1,
                    Id = x.Id,
                    IdCity = x.IdCity,
                    Latitude = (double)x.Latitude,
                    Logo = x.Logo,
                    Longitude = (double)x.Longitude,
                    WorkOnHoliDays = x.WorkOnHoliDays

                }).ToListAsync();
            }
            return companies;
        }

        public Task<Company> GetAsync(long id)
        {
            return _dbContext.CompanyQuery.Include("CompanySchedules").Include(x => x.City).Include(x => x.City.State).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<Company> GetAsync(string companyDocument)
        {
            return _dbContext.CompanyQuery.Where(x => x.CNPJ == companyDocument).FirstOrDefaultAsync();
        }
    }
}
