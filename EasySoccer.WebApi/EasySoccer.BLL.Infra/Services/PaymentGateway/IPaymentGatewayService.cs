using EasySoccer.BLL.Infra.DTO;
using EasySoccer.BLL.Infra.Services.PaymentGateway.Request;
using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Infra.Services.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        Task<TransactionResponse> PayAsync(PaymentRequest request, CompanyUser companyUser, decimal planValue, int installments, string stateCode, string cityName);
    }
}
