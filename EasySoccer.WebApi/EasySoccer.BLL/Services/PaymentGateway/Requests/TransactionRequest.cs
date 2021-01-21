namespace EasySoccer.BLL.Services.PaymentGateway.Requests
{
    public class TransactionRequest
    {
        public string api_key { get; set; }
        public int amount { get; set; }
        public string card_number { get; set; }
        public string card_cvv { get; set; }
        public string card_expiration_date { get; set; }
        public string card_holder_name { get; set; }
        public Customer customer { get; set; }
        public Billing billing { get; set; }
        public Shipping shipping { get; set; }
        public Item[] items { get; set; }
    }

    public class Customer
    {
        public string external_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public Document[] documents { get; set; }
        public string[] phone_numbers { get; set; }
        public string birthday { get; set; }
    }

    public class Document
    {
        public string type { get; set; }
        public string number { get; set; }
    }

    public class Billing
    {
        public string name { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string zipcode { get; set; }
    }

    public class Shipping
    {
        public string name { get; set; }
        public int fee { get; set; }
        public string delivery_date { get; set; }
        public bool expedited { get; set; }
        public Address address { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string title { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public bool tangible { get; set; }
    }

}
