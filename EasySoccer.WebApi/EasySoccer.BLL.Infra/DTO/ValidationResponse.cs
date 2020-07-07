using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            ErrorsMessage = new List<string>();
        }
        private List<string> ErrorsMessage { get; set; }

        public void AddValidationMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorsMessage.Add(message);
            }
        }

        public string ErrorHtmlFormatted
        {
            get
            {
                string errorHtmlFormatted = string.Empty;
                foreach (var item in ErrorsMessage)
                {
                    if (!string.IsNullOrEmpty(item))
                        errorHtmlFormatted += $"<p>{item}</p>";
                }
                return errorHtmlFormatted;
            }
        }

        public bool IsValid
        {
            get
            {
                return ErrorsMessage.Count == 0;
            }
        }
    }
}
