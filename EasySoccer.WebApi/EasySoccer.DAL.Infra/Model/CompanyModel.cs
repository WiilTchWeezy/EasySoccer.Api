using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.DAL.Infra.Model
{
    public class CompanyModel
    {
        public bool Active { get; set; }
        public string CityName { get; set; }
        public string CNPJ { get; set; }
        public List<CompanySchedule> CompanySchedules { get; set; }
        public string CompleteAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public double Distance { get; set; }
        public long Id { get; set; }
        public int? IdCity { get; set; }
        public double? Latitude { get; set; }
        public string Logo { get; set; }
        public double? Longitude { get; set; }
        public string Name { get; set; }
        public bool WorkOnHoliDays { get; set; }
    }
}
