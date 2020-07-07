using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Helper
{
    public class PasswordHelper
    {
        private static PasswordHelper _instance;
        public static PasswordHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PasswordHelper();
                return _instance;
            }
        }

        private string[] allowedEspecialChars = new string[] { "@", "!", "#", "$", "&" };
        private string[] allowedNormalChars = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z", "a", "e", "i", "o", "u" };

        public string GeneratePassword(int normalCharsLength, int especialCharsLength)
        {
            string generatedPassword = "";
            var rnd = new Random();
            for (int i = 0; i < especialCharsLength; i++)
            {
                var especialCharsNumber = rnd.Next(0, 4);
                generatedPassword += allowedEspecialChars[especialCharsNumber];
            }
            for (int i = 0; i < normalCharsLength; i++)
            {
                var normalCharsNumber = rnd.Next(0, 25);
                generatedPassword += allowedNormalChars[normalCharsNumber];
            }
            return generatedPassword;
        }
    }
}
