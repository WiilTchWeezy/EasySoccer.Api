using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.Services.Cryptography
{
    public interface ICryptographyService
    {
        string Encrypt(string decodedString);
        string Decrypt(string encodedString);
    }
}
