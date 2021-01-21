using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.Helpers
{
    public class FinancialHelper
    {
        private static FinancialHelper _instance;
        public static FinancialHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FinancialHelper();
                return _instance;
            }
        }

        public decimal GetValueFromPlan(FinancialPlanEnum financialPlan)
        {
            decimal value = 0;
            switch (financialPlan)
            {
                case FinancialPlanEnum.Mensal:
                    value = 240;
                    break;
                case FinancialPlanEnum.Semestral:
                    value = 1080;
                    break;
                case FinancialPlanEnum.Anual:
                    value = 1680;
                    break;
                case FinancialPlanEnum.Free:
                    value = 0;
                    break;
                default:
                    break;
            }
            return value;
        }

        public int GetMonthsFromPlan(FinancialPlanEnum financialPlan)
        {
            int value = 0;
            switch (financialPlan)
            {
                case FinancialPlanEnum.Mensal:
                    value = 1;
                    break;
                case FinancialPlanEnum.Semestral:
                    value = 6;
                    break;
                case FinancialPlanEnum.Anual:
                    value = 12;
                    break;
                case FinancialPlanEnum.Free:
                    value = 0;
                    break;
                default:
                    break;
            }
            return value;
        }
    }
}
