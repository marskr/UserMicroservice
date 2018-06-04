using System;
using System.IO;
using System.Security.Cryptography;
using UsersMicroservice.Logs;

namespace UsersMicroservice.Encryption
{
    class EncryptionManager
    {
        private static byte[] _salt = { 0xE1, 0xF5, 0x31, 0x36, 0xE7, 0x60, 0xC1, 0x64 };

        public string EncryptStringAES(string plainText_s, string sharedSecret_s)
        {
            if (string.IsNullOrEmpty(plainText_s))
                throw new ArgumentNullException("The text to encryption is null (plainText_s)!");
            if (string.IsNullOrEmpty(sharedSecret_s))
                throw new ArgumentNullException("The text to encryption is null (sharedSecret_s)!");

            string outStr_s = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // key generate
                Rfc2898DeriveBytes rfc_key = new Rfc2898DeriveBytes(sharedSecret_s, _salt);

                // creation of RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = rfc_key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor_ict = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream encrypt_ms = new MemoryStream())
                {
                    // prepend the IV
                    encrypt_ms.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    encrypt_ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream encrypt_cs = new CryptoStream(encrypt_ms, encryptor_ict, CryptoStreamMode.Write))
                    {
                        using (StreamWriter encrypt_sw = new StreamWriter(encrypt_cs))
                        {
                            //Write all data to the stream.
                            encrypt_sw.Write(plainText_s);
                        }
                    }
                    outStr_s = Convert.ToBase64String(encrypt_ms.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            ErrInfLogger.LockInstance.InfoLog("Encryption finished.");
            // Return the encrypted bytes from the memory stream.
            return outStr_s;
        }

        public string DecryptStringAES(string cipherText_s, string sharedSecret_s)
        {
            if (string.IsNullOrEmpty(cipherText_s))
                throw new ArgumentNullException("the text to encryption is null (cipherText_s)!");
            if (string.IsNullOrEmpty(sharedSecret_s))
                throw new ArgumentNullException("The text to encryption is null (sharedSecret_s)!");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext_s = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes rfc_key = new Rfc2898DeriveBytes(sharedSecret_s, _salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText_s);
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
                            plaintext_s = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            ErrInfLogger.LockInstance.InfoLog("Decryption finished.");
            return plaintext_s;
        }

        private static byte[] ReadByteArray(Stream stream)
        {
            byte[] by_rawLength = new byte[sizeof(int)];
            if (stream.Read(by_rawLength, 0, by_rawLength.Length) != by_rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] by_buffer = new byte[BitConverter.ToInt32(by_rawLength, 0)];
            if (stream.Read(by_buffer, 0, by_buffer.Length) != by_buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return by_buffer;
        }
    }
}
