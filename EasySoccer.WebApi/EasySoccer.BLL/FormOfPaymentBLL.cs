using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra;
using EasySoccer.DAL.Infra;
using EasySoccer.DAL.Infra.Repositories;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL
{
    public class FormOfPaymentBLL : IFormOfPaymentBLL
    {
        private IFormOfPaymentRepository _formOfPaymentRepository;
        private IEasySoccerDbContext _dbContext;
        private ICompanyRepository _companyRepository;
        public FormOfPaymentBLL(IFormOfPaymentRepository formOfPaymentRepository, ICompanyRepository companyRepository, IEasySoccerDbContext dbContext)
        {
            _formOfPaymentRepository = formOfPaymentRepository;
            _companyRepository = companyRepository;
            _dbContext = dbContext;
        }
        public async Task<FormOfPayment> CreateAsync(string name, bool active, long companyId)
        {
            var company = await _companyRepository.GetAsync(companyId);
            if (company == null)
                throw new BussinessException("Empresa não encontrada");
            var formOfPayment = new FormOfPayment
            {
                Active = active,
                CompanyId = companyId,
                CreatedDate = DateTime.UtcNow,
                Name = name
            };
            await _formOfPaymentRepository.Create(formOfPayment);
            await _dbContext.SaveChangesAsync();
            return formOfPayment;
        }

        public Task<List<FormOfPayment>> GetAsync(long companyId)
        {
            return _formOfPaymentRepository.GetAsync(companyId);
        }

        public Task<List<FormOfPayment>> GetAsync(int page, int pageSize, long companyId)
        {
            return _formOfPaymentRepository.GetAsync(companyId, page, pageSize);
        }

        public Task<int> GetTotalAsync(long companyId)
        {
            return _formOfPaymentRepository.GetTotalAsync(companyId);
        }

        public async Task<FormOfPayment> UpdateAsync(int formOfPaymentId, string name, bool active, long companyId)
        {
            var formOfPayment = await _formOfPaymentRepository.GetAsync(formOfPaymentId);
            if (formOfPayment == null)
                throw new BussinessException("Forma de pagamento não encontrada");
            if(formOfPayment.CompanyId != companyId)
                throw new BussinessException("Empresa não é válida");

            formOfPayment.Name = name;
            formOfPayment.Active = active;
            await _formOfPaymentRepository.Edit(formOfPayment);
            await _dbContext.SaveChangesAsync();
            return formOfPayment;
        }
    }
}
