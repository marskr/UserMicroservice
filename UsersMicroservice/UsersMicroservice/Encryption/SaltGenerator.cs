using System;
using System.Security.Cryptography;

namespace UsersMicroservice.Encryption
{
    public class SaltGenerator
    {
        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider rncCsp = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[8];
            rncCsp.GetBytes(buffer);
            
            return BitConverter.ToString(buffer);
        }
    }
}
