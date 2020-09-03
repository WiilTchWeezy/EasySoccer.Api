using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class GetSchedulesResponse
    {
        public string Hour { get; set; }
        public List<GetSchedulesResponseEvents> Events { get; set; }

        public TimeSpan HourSpan { get; set; }
        public GetSchedulesResponse()
        {
            Events = new List<GetSchedulesResponseEvents>();
        }
    }
}
