using System;

namespace EasySoccer.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(object obj, object key) : base(String.Format("Not found {0} by the key {1}", obj.GetType().Name, key))
        {

        }
    }
}
