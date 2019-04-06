using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ISoccerPitchReservationBLL
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, int companyId, int page, int pageSize);
    }
}
