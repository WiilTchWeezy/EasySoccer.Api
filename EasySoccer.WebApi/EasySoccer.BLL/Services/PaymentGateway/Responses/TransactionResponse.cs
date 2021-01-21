using System;

namespace EasySoccer.BLL.Services.PaymentGateway.Responses
{

    public class TransactionResponse
    {
        public string _object { get; set; }
        public string status { get; set; }
        public object refse_reason { get; set; }
        public string status_reason { get; set; }
        public string acquirer_response_code { get; set; }
        public string acquirer_name { get; set; }
        public string acquirer_id { get; set; }
        public string authorization_code { get; set; }
        public object soft_descriptor { get; set; }
        public int tid { get; set; }
        public int nsu { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public int amount { get; set; }
        public int authorized_amount { get; set; }
        public int paid_amount { get; set; }
        public int refunded_amount { get; set; }
        public int installments { get; set; }
        public int id { get; set; }
        public int cost { get; set; }
        public string card_holder_name { get; set; }
        public string card_last_digits { get; set; }
        public string card_first_digits { get; set; }
        public string card_brand { get; set; }
        public object card_pin_mode { get; set; }
        public object postback_url { get; set; }
        public string payment_method { get; set; }
        public string capture_method { get; set; }
        public object antifraud_score { get; set; }
        public object boleto_url { get; set; }
        public object boleto_barcode { get; set; }
        public object boleto_expiration_date { get; set; }
        public string referer { get; set; }
        public string ip { get; set; }
        public object subscription_id { get; set; }
        public object phone { get; set; }
        public object address { get; set; }
        public Customer customer { get; set; }
        public Billing billing { get; set; }
        public Shipping shipping { get; set; }
        public Item[] items { get; set; }
        public Card card { get; set; }
        public object split_rules { get; set; }
        public Metadata metadata { get; set; }
        public Antifraud_Metadata antifraud_metadata { get; set; }
        public object reference_key { get; set; }
    }

    public class Customer
    {
        public string _object { get; set; }
        public int id { get; set; }
        public string external_id { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public object document_number { get; set; }
        public string document_type { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string[] phone_numbers { get; set; }
        public object born_at { get; set; }
        public string birthday { get; set; }
        public object gender { get; set; }
        public DateTime date_created { get; set; }
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string _object { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string number { get; set; }
    }

    public class Billing
    {
        public Address address { get; set; }
        public string _object { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Address
    {
        public string _object { get; set; }
        public string street { get; set; }
        public object complementary { get; set; }
        public string street_number { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public int id { get; set; }
    }

    public class Shipping
    {
        public Address1 address { get; set; }
        public string _object { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int fee { get; set; }
        public string delivery_date { get; set; }
        public bool expedited { get; set; }
    }

    public class Address1
    {
        public string _object { get; set; }
        public string street { get; set; }
        public object complementary { get; set; }
        public string street_number { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public int id { get; set; }
    }

    public class Card
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
        public bool valid { get; set; }
        public string expiration_date { get; set; }
    }

    public class Metadata
    {
    }

    public class Antifraud_Metadata
    {
    }

    public class Item
    {
        public string _object { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public object category { get; set; }
        public bool tangible { get; set; }
        public object venue { get; set; }
        public object date { get; set; }
    }

}
