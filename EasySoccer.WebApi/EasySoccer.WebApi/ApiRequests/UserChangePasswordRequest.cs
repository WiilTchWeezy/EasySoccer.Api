namespace EasySoccer.WebApi.ApiRequests
{
    public class UserChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
