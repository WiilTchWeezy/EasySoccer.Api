using EasySoccer.BLL.Infra;
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
        public SoccerPitchBLL(ISoccerPitchRepository soccerPitchRepository)
        {
            _soccerPitchRepository = soccerPitchRepository;
        }
        public Task<List<SoccerPitch>> GetAsync(int page, int pageSize)
        {
            return _soccerPitchRepository.GetAsync(page, pageSize);
        }
    }
}
