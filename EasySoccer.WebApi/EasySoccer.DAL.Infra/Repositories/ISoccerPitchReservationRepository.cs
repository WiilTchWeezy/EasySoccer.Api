﻿using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.DAL.Infra.Repositories
{
    public interface ISoccerPitchReservationRepository
    {
        Task<List<SoccerPitchReservation>> GetAsync(DateTime date, long[] soccerPitchs, int page, int pageSize);
    }
}
