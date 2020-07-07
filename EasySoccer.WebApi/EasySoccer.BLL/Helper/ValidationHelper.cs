using EasySoccer.BLL.Infra.DTO;
using EasySoccer.DAL.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Helper
{
    public class ValidationHelper
    {
        private static ValidationHelper instance;
        public static ValidationHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new ValidationHelper();
                return instance;
            }
        }

        public async Task<ValidationResponse> Validate(FormInputCompanyEntryRequest entity, ICompanyUserRepository companyUserRepository, ICompanyRepository companyRepository)
        {
            var validationResponse = new ValidationResponse();
            validationResponse.AddValidationMessage(this.ValidateEmail(entity.UserEmail));
            validationResponse.AddValidationMessage(this.ValidateCompanyDocument(entity.CompanyDocument));
            validationResponse.AddValidationMessage(this.ValidateCardNumberAndSecurityCode(entity.CardNumber, entity.SecurityCode));
            validationResponse.AddValidationMessage(this.ValidateUserDocument(entity.FinancialDocument));
            validationResponse.AddValidationMessage(await this.ValidateAlreadyExistEmail(entity.UserEmail, companyUserRepository));
            validationResponse.AddValidationMessage(await this.ValidateAlreadyExistCompanyDocument(entity.CompanyDocument, companyRepository));
            return validationResponse;
        }

        private string ValidateEmail(string email)
        {
            var emailAttr = new EmailAddressAttribute();
            if (emailAttr.IsValid(email) == false)
                return "Email inválido";
            return String.Empty;
        }

        private string ValidateCompanyDocument(string companyDocument)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            companyDocument = companyDocument.Trim();
            companyDocument = companyDocument.Replace(".", "").Replace("-", "").Replace("/", "");
            if (companyDocument.Length != 14)
                return "CNPJ inválido";
            tempCnpj = companyDocument.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            if (companyDocument.EndsWith(digito) == false)
                return "CNPJ inválido";
            return String.Empty;
        }

        private string ValidateUserDocument(string userDocument)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            userDocument = userDocument.Trim();
            userDocument = userDocument.Replace(".", "").Replace("-", "");
            if (userDocument.Length != 11)
                return "CPF de cobrança inválido";
            tempCpf = userDocument.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            if (userDocument.EndsWith(digito) == false)
                return "CPF de cobrança inválido";
            return String.Empty;
        }

        private string ValidateCardNumberAndSecurityCode(string cardNumber, string securityCode)
        {
            if (cardNumber.Length < 16)
                return "Cartão de crédito inválido.";
            if (securityCode.Length < 3)
                return "Código de segurança inválido.";
            return String.Empty;
        }

        private async Task<string> ValidateAlreadyExistEmail(string email, ICompanyUserRepository companyUserRepository)
        {
            var currentUser = await companyUserRepository.GetAsync(email);
            if (currentUser != null)
                return $"Já existe um usuário cadastrado com o e-mail {email}";
            return String.Empty;
        }

        private async Task<string> ValidateAlreadyExistCompanyDocument(string companyDocument, ICompanyRepository companyRepository)
        {
            var currentCompany = await companyRepository.GetAsync(companyDocument);
            if (currentCompany != null)
                return $"Já existe uma empresa cadastrada com o CNPJ {companyDocument}";
            return String.Empty;
        }
    }
}
