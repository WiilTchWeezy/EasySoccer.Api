using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IUserBLL
    {
        Task<PersonUserResponse> LoginAsync(string email, string password);
        Task<List<PersonUserResponse>> GetAsync(string filter);
        Task<PersonUserResponse> CreatePersonAsync(string name, string phone, string email, CreatedFromEnum createdFrom);
        Task<PersonUserResponse> LoginFromFacebookAsync(string email, string id, string name, string birthday);
        Task<bool> ChangeUserPassword(string oldPassword, Guid userId, string newPassword);
        Task<PersonUserResponse> GetAsync(Guid userId);
        Task<PersonUserResponse> UpdateAsync(Guid userId, string name, string email, string phoneNumber);
    }
}
