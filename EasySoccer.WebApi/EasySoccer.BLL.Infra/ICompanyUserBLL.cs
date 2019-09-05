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
    }
}
