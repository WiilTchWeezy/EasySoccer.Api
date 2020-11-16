using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.BLL.Infra.Services.Azure;
using EasySoccer.BLL.Infra.Services.Azure.Enums;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class SoccerPitchBLL : ISoccerPitchBLL
    {
        private ISoccerPitchRepository _soccerPitchRepository;
        private IEasySoccerDbContext _dbContext;
        private ISoccerPitchSoccerPitchPlanRepository _soccerPitchSoccerPitchPlanRepository;
        private ISportTypeRepository _sportTypeRepository;
        private IBlobStorageService _blobStorageService;
        public SoccerPitchBLL(ISoccerPitchRepository soccerPitchRepository, ISoccerPitchSoccerPitchPlanRepository soccerPitchSoccerPitchPlanRepository, IEasySoccerDbContext dbContext, ISportTypeRepository sportTypeRepository, IBlobStorageService blobStorageService)
        {
            _soccerPitchRepository = soccerPitchRepository;
            _soccerPitchSoccerPitchPlanRepository = soccerPitchSoccerPitchPlanRepository;
            _dbContext = dbContext;
            _sportTypeRepository = sportTypeRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<SoccerPitch> CreateAsync(string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int[] soccerPitchPlansId, int sportTypeId, int interval, string color)
        {
            var soccerPitch = new SoccerPitch
            {
                Active = active,
                ActiveDate = active ? (DateTime?)DateTime.UtcNow : null,
                CompanyId = companyId,
                CreatedDate = DateTime.UtcNow,
                Description = description,
                HasRoof = hasRoof,
                InactiveDate = active == false ? (DateTime?)DateTime.UtcNow : null,
                Name = name,
                NumberOfPlayers = numberOfPlayers,
                SportTypeId = sportTypeId,
                Interval = interval,
                Color = color
            };
            await _soccerPitchRepository.Create(soccerPitch);
            foreach (var item in soccerPitchPlansId)
            {
                await _soccerPitchSoccerPitchPlanRepository.Create(new SoccerPitchSoccerPitchPlan
                {
                    CreatedDate = DateTime.UtcNow,
                    SoccerPitchPlanId = item,
                    SoccerPitchId = soccerPitch.Id
                });
            }
            await _dbContext.SaveChangesAsync();
            return soccerPitch;
        }

        public Task<List<SoccerPitch>> GetAsync(int page, int pageSize, long companyId)
        {
            return _soccerPitchRepository.GetAsync(page, pageSize, companyId);
        }

        public Task<List<SportType>> GetSportTypeAsync()
        {
            return _sportTypeRepository.GetAsync();
        }

        public Task<List<SportType>> GetSportTypeAsync(long companyId)
        {
            return _sportTypeRepository.GetAsync(companyId);
        }

        public Task<int> GetTotalAsync()
        {
            return _soccerPitchRepository.GetTotalAsync();
        }

        public async Task SaveImageAsync(long companyId, long soccerPitchId, string imageBase64)
        {
            var soccerPitch = await _soccerPitchRepository.GetAsync(companyId, soccerPitchId);
            if (soccerPitch == null)
                throw new BussinessException("Quadra não encontrada!");
            if (string.IsNullOrEmpty(soccerPitch.ImageName) == false)
            {
                try
                {
                    _blobStorageService.Delete(soccerPitch.ImageName, BlobContainerEnum.SoccerPitchContainer);
                }
                catch (Exception)
                {

                }
            }
            var bytes = Convert.FromBase64String(imageBase64);
            var fileName = await _blobStorageService.Save(bytes, BlobContainerEnum.SoccerPitchContainer);
            soccerPitch.ImageName = fileName;
            await _soccerPitchRepository.Edit(soccerPitch);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SoccerPitch> UpdateAsync(long id, string name, string description, bool hasRoof, int numberOfPlayers, long companyId, bool active, int[] soccerPitchPlansId, int sportTypeId, int interval, string color)
        {
            var soccerPitch = await _soccerPitchRepository.GetAsync(id);
            if (soccerPitch == null)
                throw new NotFoundException(soccerPitch, id);

            var currentPlans = await _soccerPitchSoccerPitchPlanRepository.GetAsync(id);
            if (currentPlans != null && currentPlans.Count > 0)
            {
                var plansToDelete = currentPlans.Where(x => soccerPitchPlansId.Contains(x.SoccerPitchPlanId) == false).ToList();
                foreach (var item in plansToDelete)
                {
                    await _soccerPitchSoccerPitchPlanRepository.Delete(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            var currentPlansIds = currentPlans.Select(x => x.SoccerPitchPlanId).ToList();
            var plansToAdd = soccerPitchPlansId.Where(x => currentPlansIds.Contains(x) == false).ToList();
            foreach (var item in plansToAdd)
            {
                await _soccerPitchSoccerPitchPlanRepository.Create(new SoccerPitchSoccerPitchPlan { SoccerPitchPlanId = item, SoccerPitchId = id, CreatedDate = DateTime.Now });
            }
            await _dbContext.SaveChangesAsync();

            soccerPitch.Name = name;
            soccerPitch.Description = description;
            soccerPitch.HasRoof = hasRoof;
            soccerPitch.InactiveDate = active == false ? (DateTime?)DateTime.UtcNow : null;
            soccerPitch.NumberOfPlayers = numberOfPlayers;
            soccerPitch.CompanyId = companyId;
            soccerPitch.SportTypeId = sportTypeId;
            soccerPitch.Active = active;
            soccerPitch.Interval = interval;
            soccerPitch.ActiveDate = active ? (DateTime?)DateTime.UtcNow : null;
            soccerPitch.Color = color;
            await _soccerPitchRepository.Edit(soccerPitch);
            await _dbContext.SaveChangesAsync();
            return soccerPitch;
        }
    }
}
