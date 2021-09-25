using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Helper
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
        public string GetEnumDescription(LimitTypeEnum limitTypeEnum)
        {
            string enumDescription = string.Empty;
            switch (limitTypeEnum)
            {
                case LimitTypeEnum.DaysAfterFirstReservation:
                    enumDescription = "Dias após primeira reserva";
                    break;
                case LimitTypeEnum.TotalReservations:
                    enumDescription = "Total de reservas";
                    break;
                case LimitTypeEnum.FillCurrentMonth:
                    enumDescription = "Preencher mês atual";
                    break;
                default:
                    break;
            }
            return enumDescription;
        }

        public Dictionary<int, string> GetEnumDictionary<T>() where T : System.Enum
        {
            var response = new Dictionary<int, string>();
            var values = Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (typeof(T) == typeof(LimitTypeEnum))
                    response.Add((int)item, GetEnumDescription((LimitTypeEnum)item));
            }
            return response;
        }
    }
}
