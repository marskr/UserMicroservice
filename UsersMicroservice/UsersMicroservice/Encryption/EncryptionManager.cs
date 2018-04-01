using System;
using System.IO;
using System.Security.Cryptography;

namespace UsersMicroservice.Encryption
{
    class EncryptionManager
    {
        private static byte[] _salt = { 0xE1, 0xF5, 0x31, 0x36, 0xE7, 0x60, 0xC1, 0x64 };

        public string EncryptStringAES(string s_plainText, string s_sharedSecret)
        {
            if (string.IsNullOrEmpty(s_plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(s_sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string s_outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // key generate
                Rfc2898DeriveBytes rfc_key = new Rfc2898DeriveBytes(s_sharedSecret, _salt);

                // creation of RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = rfc_key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform ict_encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream ms_Encrypt = new MemoryStream())
                {
                    // prepend the IV
                    ms_Encrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    ms_Encrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream cs_Encrypt = new CryptoStream(ms_Encrypt, ict_encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(cs_Encrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(s_plainText);
                        }
                    }
                    s_outStr = Convert.ToBase64String(ms_Encrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return s_outStr;
        }

        public string DecryptStringAES(string s_cipherText, string s_sharedSecret)
        {
            if (string.IsNullOrEmpty(s_cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(s_sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string s_plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes rfc_key = new Rfc2898DeriveBytes(s_sharedSecret, _salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(s_cipherText);
                using (MemoryStream ms_Decrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = rfc_key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(ms_Decrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform ict_decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream cs_Decrypt = new CryptoStream(ms_Decrypt, ict_decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(cs_Decrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            s_plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return s_plaintext;
        }

        private static byte[] ReadByteArray(Stream s_stream)
        {
            byte[] by_rawLength = new byte[sizeof(int)];
            if (s_stream.Read(by_rawLength, 0, by_rawLength.Length) != by_rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] by_buffer = new byte[BitConverter.ToInt32(by_rawLength, 0)];
            if (s_stream.Read(by_buffer, 0, by_buffer.Length) != by_buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return by_buffer;
        }
    }
}
