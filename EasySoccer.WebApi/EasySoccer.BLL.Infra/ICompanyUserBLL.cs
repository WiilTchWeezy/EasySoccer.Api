using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface ICompanyUserBLL
    {
        Task<CompanyUser> LoginAsync(string email, string password);
        Task<CompanyUser> CreateAsync(string name, string email, string phone, string password, long companyId);

        Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword);
    }
}
