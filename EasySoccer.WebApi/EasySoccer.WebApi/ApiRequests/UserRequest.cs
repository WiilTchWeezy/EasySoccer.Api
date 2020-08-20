namespace EasySoccer.WebApi.ApiRequests
{
    public class UserRequest
    {
        public string Name { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string SocialMediaId { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public bool CreatedFromWeb { get; set; } = false;
    }
}
