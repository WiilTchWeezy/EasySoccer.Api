using Microsoft.EntityFrameworkCore.Internal;
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

        public void AddValidationMessage(List<string> messages)
        {
            if (messages != null && messages.Any())
            {
                foreach (var item in messages)
                {
                    if (string.IsNullOrEmpty(item) == false)
                        ErrorsMessage.Add(item);
                }
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

        public string ErrorFormatted
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var item in ErrorsMessage)
                {
                    if (!string.IsNullOrEmpty(item))
                        sb.AppendLine(item);
                }
                return sb.ToString();
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
