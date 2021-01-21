using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Services.PaymentGateway.Responses
{
    public class CardResponse
    {
        public string _object { get; set; }
        public string id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public string brand { get; set; }
        public string holder_name { get; set; }
        public string first_digits { get; set; }
        public string last_digits { get; set; }
        public string country { get; set; }
        public string fingerprint { get; set; }
        public object customer { get; set; }
        public bool valid { get; set; }
        public string expiration_date { get; set; }
    }
}
