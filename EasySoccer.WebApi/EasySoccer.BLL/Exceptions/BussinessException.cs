using Newtonsoft.Json;
using System;

namespace EasySoccer.BLL.Exceptions
{
    public class BussinessException : Exception
    {
        [JsonIgnore]
        public Exception Exception { get; set; }
        public BussinessException(string message)
        {
            Exception = new Exception(message);
        }


        public override string Message
        {
            get
            {
                return Exception?.Message;
            }
        }


    }
}
