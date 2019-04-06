using EasySoccer.BLL.Infra;
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
        public SoccerPitchReservationBLL(ISoccerPitchReservationRepository soccerPitchReservationRepository, ISoccerPitchRepository soccerPitchRepository)
        {
            _soccerPitchReservationRepository = soccerPitchReservationRepository;
            _soccerPitchRepository = soccerPitchRepository;
        }
        public async Task<List<SoccerPitchReservation>> GetAsync(DateTime date, int companyId, int page, int pageSize)
        {
            var companyPitchs = await _soccerPitchRepository.GetAsync(companyId);
            return await _soccerPitchReservationRepository.GetAsync(date, companyPitchs, page, pageSize);
        }
    }
}
