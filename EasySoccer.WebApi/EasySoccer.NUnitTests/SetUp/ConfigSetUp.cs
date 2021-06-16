using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Test.SetUp
{
    public class ConfigSetUp
    {
        private static ConfigSetUp _instance;
        public static ConfigSetUp Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigSetUp();
                return _instance;
            }
        }

        public IConfiguration SetUpConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"ConnectionStrings:AzureCloudStorageConnection", "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint = http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint = http://127.0.0.1:10001/devstoreaccount1;TableEndpoint = http://127.0.0.1:10002/devstoreaccount1;"},
                {"GeneralConfig:EncryptKey", "eaassoccerencryptKeyKeyKeyKeyKey"},
                {"FinancialConfiguration:DaysFree", "15"}
            
            };
            return new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
        }
    }
}
