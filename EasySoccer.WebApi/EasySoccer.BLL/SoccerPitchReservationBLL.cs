using EasySoccer.BLL.Enums;
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
    public class SoccerPitchReservationBLL : ISoccerPitchReservationBLL
    {
        private ISoccerPitchReservationRepository _soccerPitchReservationRepository;
        private ISoccerPitchRepository _soccerPitchRepository;
        private IEasySoccerDbContext _dbContext;
        public SoccerPitchReservationBLL(ISoccerPitchReservationRepository soccerPitchReservationRepository, ISoccerPitchRepository soccerPitchRepository, IEasySoccerDbContext dbContext)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
            _dbContext = dbContext;
        }

        public async Task<SoccerPitchReservation> CreateAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note, long companyUserId)
        {
            var soccerPitchReservation = new SoccerPitchReservation
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Note = note,
                SelectedDate = selectedDate,
                SelectedHourEnd = hourFinish,
                SelectedHourStart = hourStart,
                SoccerPitchId = soccerPitchId,
                Status = StatusEnum.AguardandoAprovacao,
                StatusChangedUserId = companyUserId,
                UserId = userId
            };
            await _soccerPitchReservationRepository.Create(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }

        public async Task<List<SoccerPitchReservation>> GetAsync(DateTime date, int companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(date, companyPitchs, page, pageSize);
        }

        public async Task<SoccerPitchReservation> UpdateAsync(Guid id, long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourFinish, string note)
        {
            var soccerPitchReservation = await _soccerPitchReservationRepository.GetAsync(id);
            if (soccerPitchReservation == null)
                throw new NotFoundException(soccerPitchReservation, id);
            soccerPitchReservation.Note = note;
            soccerPitchReservation.SelectedDate = selectedDate;
            soccerPitchReservation.SelectedHourEnd = hourFinish;
            soccerPitchReservation.SelectedHourStart = hourStart;
            soccerPitchReservation.SoccerPitchId = soccerPitchId;
            soccerPitchReservation.UserId = userId;
            await _soccerPitchReservationRepository.Edit(soccerPitchReservation);
            await _dbContext.SaveChangesAsync();
            return soccerPitchReservation;
        }
    }
}
