namespace EasySoccer.WebApi.ApiRequests
{
    public class CompanyRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CNPJ { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool WorkOnHolidays { get; set; }
    }
}
