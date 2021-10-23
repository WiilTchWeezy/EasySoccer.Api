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

        public string GetApplicationEnumDescription(ApplicationEnum application)
        {
            string applicationText = string.Empty;
            switch (application)
            {
                case ApplicationEnum.Unknow:
                    applicationText = "Desconhecido";
                    break;
                case ApplicationEnum.WebApp:
                    applicationText = "Plataforma Web";
                    break;
                case ApplicationEnum.MobileAdm:
                    applicationText = "Aplicativo de Gerenciamento";
                    break;
                case ApplicationEnum.MobileUser:
                    applicationText = "Aplicativo de Reservas Online";
                    break;
                default:
                    break;
            }
            return applicationText;
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

        public string GetEnumDescription(PaymentStatusEnum paymentStatus)
        {
            string enumDescription = string.Empty;
            switch (paymentStatus)
            {
                case PaymentStatusEnum.Created:
                    enumDescription = "Confirmado";
                    break;
                case PaymentStatusEnum.Canceled:
                    enumDescription = "Cancelado";
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
