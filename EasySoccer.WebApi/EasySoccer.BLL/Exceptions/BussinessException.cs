using Newtonsoft.Json;
using System;

namespace EasySoccer.BLL.Exceptions
{
    public class BussinessException
    {
        [JsonIgnore]
        public Exception Exception { get; set; }
        public BussinessException(string message)
        {
            Exception = new Exception(message);
        }

        public string Message
        {
            get
            {
                return Exception?.Message;
            }
        }


    }
}
