using System;

namespace EasySoccer.BLL.Exceptions
{
    public class BussinessException : Exception
    {
        public BussinessException(string message) : base(message)
        {

        }
    }
}
