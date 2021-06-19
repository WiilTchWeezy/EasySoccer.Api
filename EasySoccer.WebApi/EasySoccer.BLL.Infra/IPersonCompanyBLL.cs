using EasySoccer.BLL.Infra.DTO;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra
{
    public interface IPersonCompanyBLL
    {
        Task<List<PersonCompany>> GetAsync(string name, string email, string phone, int page, int pageSize, long companyId);
        Task<List<PersonCompany>> GetAutoCompleteAsync(string filter, long companyId);
        Task<PersonCompany> CreateAsync(string name, string email, string phone, long companyId);
        Task<PersonCompany> UpdateAsync(Guid personId, string name, string email, string phone, long companyId);
        Task<int> GetAsync(string name, string email, string phone, long companyId);
        Task<PersonCompanyInfoResponse> GetInfoAsync(Guid id);
    }
}
