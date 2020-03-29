using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class CompanySchedulesRequest
    {
        public long CompanyId { get; set; }
        public int Day { get; set; }
        public long FinalHour { get; set; }
        public long StartHour { get; set; }
        public bool WorkOnThisDay { get; set; }
    }


}
