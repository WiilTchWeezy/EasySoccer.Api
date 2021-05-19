using EasySoccer.BLL.Infra.DTO;
using System.Collections.Generic;

namespace EasySoccer.WebApi.ApiRequests
{
    public class CompanyRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CNPJ { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? IdCity { get; set; }
        public bool WorkOnHolidays { get; set; }
        public string CompleteAddress { get; set; }
        public bool InsertReservationConfirmed { get; set; }
        public List<CompanySchedulesRequest> CompanySchedules { get; set; }
    }
}
