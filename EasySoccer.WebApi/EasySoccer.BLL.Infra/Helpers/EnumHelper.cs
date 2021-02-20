using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.Helpers
{
    public class EnumHelper
    {
        private static EnumHelper _instance;
        public static EnumHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EnumHelper();
                return _instance;
            }
        }

        public string GetStatusEnumDescription(StatusEnum status)
        {
            string statusText = string.Empty;
            switch (status)
            {
                case StatusEnum.Waiting:
                    statusText = "Aguardando confirmação";
                    break;
                case StatusEnum.Canceled:
                    statusText = "Cancelado";
                    break;
                case StatusEnum.Confirmed:
                    statusText = "Confirmado";
                    break;
                case StatusEnum.Concluded:
                    statusText = "Finalizado";
                    break;
                default:
                    break;
            }
            return statusText;
        }
    }
}
