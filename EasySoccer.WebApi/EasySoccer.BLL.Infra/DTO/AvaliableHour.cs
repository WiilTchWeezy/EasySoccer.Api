using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class AvaliableHour
    {
        public int Hour { get; set; }
        public TimeSpan HourStart { get; set; }
        public TimeSpan HourEnd { get; set; }
    }
}
