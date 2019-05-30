using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class SoccerPitchBLL : ISoccerPitchBLL
    {
        private ISoccerPitchRepository _soccerPitchRepository;
        private IEasySoccerDbContext _dbContext;
        public SoccerPitchBLL(ISoccerPitchRepository soccerPitchRepository, IEasySoccerDbContext dbContext)
        {
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
        }

        public async Task<SoccerPitch> CreateAsync(string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int soccerPitchPlanId)
        {
            var soccerPitch = new SoccerPitch
            {
                Active = active,
                ActiveDate = active ? (DateTime?)DateTime.UtcNow : null ,
                CompanyId = companyId,
                CreatedDate = DateTime.UtcNow,
                Description = description,
                HasRoof = hasRoof,
                InactiveDate = active == false ? (DateTime?)DateTime.UtcNow : null,
                Name = name,
                NumberOfPlayers = numberOfPlayers
            };
            await _soccerPitchRepository.Create(soccerPitch);
            await _dbContext.SaveChangesAsync();
            return soccerPitch;
        }

        public Task<List<SoccerPitch>> GetAsync(int page, int pageSize)
        {
            return _soccerPitchRepository.GetAsync(page, pageSize);
        }

        public async Task<SoccerPitch> UpdateAsync(long id, string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int soccerPitchPlanId)
        {
            var soccerPitch = await _soccerPitchRepository.GetAsync(id);
            if (soccerPitch == null)
                throw new NotFoundException(soccerPitch, id);

            soccerPitch.Name = name;
            soccerPitch.Description = description;
            soccerPitch.HasRoof = hasRoof;
            soccerPitch.InactiveDate = active == false ? (DateTime?)DateTime.UtcNow : null;
            soccerPitch.NumberOfPlayers = numberOfPlayers;
            soccerPitch.CompanyId = companyId;
            soccerPitch.Active = active;
            soccerPitch.ActiveDate = active ? (DateTime?)DateTime.UtcNow : null;
            await _soccerPitchRepository.Edit(soccerPitch);
            await _dbContext.SaveChangesAsync();
            return soccerPitch;
        }
    }
}
