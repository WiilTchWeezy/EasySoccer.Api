using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Services.PaymentGateway.Requests
{
    public class CardRequest
    {
        public string api_key { get; set; }
        public string card_expiration_date { get; set; }
        public string card_number { get; set; }
        public string card_cvv { get; set; }
        public string card_holder_name { get; set; }
    }
}
