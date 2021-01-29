using EasySoccer.BLL.Exceptions;
using EasySoccer.BLL.Infra.DTO;
using EasySoccer.BLL.Infra.Services.PaymentGateway;
using EasySoccer.BLL.Infra.Services.PaymentGateway.Request;
using EasySoccer.BLL.Services.PaymentGateway.Requests;
using EasySoccer.BLL.Services.PaymentGateway.Responses;
using EasySoccer.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.BLL.Services.PaymentGateway
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private string _key = "";
        private string _encryptionKey = "";
        private string _apiUrl = "";
        public PaymentGatewayService(IConfiguration configuration)
        {
            var config = configuration.GetSection("PaymentGatewayConfig");
            if (config != null)
            {
                _key = config.GetValue<string>("ApiKey");
                _encryptionKey = config.GetValue<string>("ApiEncryption");
                _apiUrl = config.GetValue<string>("ApiUrl");
            }
        }

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            return httpClient;
        }

        private async Task<CardResponse> CreateCardAsync(string cardNumber, string securityCode, string cardExpiration, string financialName)
        {
            var cardResponse = new CardResponse();
            var request = new CardRequest
            {
                api_key = _key,
                card_number = cardNumber,
                card_cvv = securityCode,
                card_holder_name = financialName,
                card_expiration_date = cardExpiration.Replace("20", string.Empty)
            };
            using (var httpClient = CreateClient())
            {
                var httpResponse = await httpClient.PostAsJsonAsync("1/cards", request);
                var response = await httpResponse.Content.ReadAsStringAsync();
                cardResponse = JsonConvert.DeserializeObject<CardResponse>(response, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            return cardResponse;
        }

        private async Task<List<CardResponse>> GetCards(string financialName, string cardNumber)
        {
            var cardResponseList = new List<CardResponse>();
            using (var httpClient = CreateClient())
            {
                var httpResponse = await httpClient.GetAsync($"1/cards?api_key={_key}&holder_name={financialName}&first_digits={cardNumber.Substring(0, 5)}");
                var response = await httpResponse.Content.ReadAsStringAsync();
                cardResponseList = JsonConvert.DeserializeObject<List<CardResponse>>(response, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            return cardResponseList;
        }

        private async Task<Transaction> CreateTransactionAsync(decimal value, string cardHash, CompanyUser companyUser, PaymentRequest request, string stateCode, string cityName)
        {
            try
            {
                var transationResponse = new TransactionResponse();
                var amount = (int)value;
                PagarMeService.DefaultApiKey = _key;
                PagarMeService.DefaultEncryptionKey = _encryptionKey;
                Transaction transaction = new Transaction();
                DateTime birthDay = DateTime.Now;
                DateTime.TryParse(request.FinancialBirthDay, out birthDay);
                transaction.Amount = amount;
                transaction.Installments = request.SelectedInstallments;
                transaction.Card = new PagarMe.Card
                {
                    Id = cardHash
                };
                transaction.Customer = new PagarMe.Customer()
                {
                    ExternalId = companyUser.Id.ToString(),
                    Name = request.FinancialName,
                    Type = CustomerType.Individual,
                    Country = "br",
                    Email = companyUser.Email,
                    Documents = new[]
                    {
                    new PagarMe.Document
                    {
                        Type = DocumentType.Cpf,
                        Number = request.FinancialDocument
                    }
                },
                    Birthday = birthDay.ToString("yyyy-MM-dd")
                };
                if (string.IsNullOrEmpty(companyUser.Phone) == false)
                {
                    transaction.Customer.PhoneNumbers = new string[]
                    {
                    string.IsNullOrEmpty(companyUser.Phone) ? string.Empty : "+55" + companyUser.Phone.Replace(" ","").Replace("(", "").Replace(")", "").Replace("-", "")
                    };
                }
                transaction.Billing = new PagarMe.Billing
                {
                    Name = request.FinancialName,
                    Address = new PagarMe.Address
                    {
                        Country = "br",
                        State = stateCode,
                        City = cityName,
                        Neighborhood = request.Neighborhood,
                        Street = request.Street,
                        StreetNumber = request.StreetNumber,
                        Zipcode = request.ZipCode,
                        Complementary = request.Complementary
                    }
                };
                transaction.Item = new[]
                {
                  new PagarMe.Item()
                  {
                    Id = "1",
                    Title = "Licença EasySoccer",
                    Quantity = 1,
                    Tangible = false,
                    UnitPrice = amount
                  }
            };
                await transaction.SaveAsync();
                return transaction;
            }
            catch (PagarMe.PagarMeException pgmeEx)
            {
                if (pgmeEx.Error != null && pgmeEx.Error.Errors != null && pgmeEx.Error.Errors.Count() > 0)
                {
                    throw new BussinessException(string.Join(" - ", pgmeEx.Error.Errors.Select(x => x.Message).ToArray()));
                }
                else
                {
                    throw pgmeEx;
                }
            }
        }


        public async Task<TransactionResponse> PayAsync(PaymentRequest request, CompanyUser companyUser, decimal planValue, int installments, string cityName, string stateCode)
        {
            TransactionResponse transactionResponse = null;
            CardResponse card = null;
            var cards = await GetCards(request.FinancialName, request.CardNumber);
            if (cards != null && cards.Count > 0 && cards.Count == 1)
            {
                card = cards.FirstOrDefault();
            }
            if (card == null)
                card = await this.CreateCardAsync(request.CardNumber, request.SecurityCode, request.CardExpiration, request.FinancialName);
            if (card != null && string.IsNullOrEmpty(card.id) == false)
            {
                int amount = (int)planValue * 100;
                var transaction = await CreateTransactionAsync(amount, card.id, companyUser, request, stateCode, cityName);
                if (transaction != null)
                {
                    transactionResponse = new TransactionResponse();
                    transactionResponse.IsAuthorized = transaction.Status == TransactionStatus.Paid;
                    transactionResponse.TransactionJson = JsonConvert.SerializeObject(new { transaction.AuthorizationCode, transaction.Nsu, transaction.Id });
                    transactionResponse.Status = (int)transaction.Status;
                }
            }
            return transactionResponse;
        }
    }
}
