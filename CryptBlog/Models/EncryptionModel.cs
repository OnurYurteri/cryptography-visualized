using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using CryptBlog.Entities;
using CryptBlog.Models;
using System.Security.Cryptography;

namespace CryptBlog.Models
{
    public class EncryptionModel
    {
        class AES_EncryptionInstance
        {
            public byte[] encrypted;
            public byte[] key;
            public byte[] IV;
            public AES_EncryptionInstance(byte[] _encrypted, byte[] _key, byte[] _IV)
            {
                this.encrypted = _encrypted;
                this.key = _key;
                this.IV = _IV;
            }
        }

        private AES_EncryptionInstance AES_EncryptString(string text, byte[] key)
        {
            byte[] encrypted;
            Aes aesAlg = Aes.Create();
            aesAlg.Key = key;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return new AES_EncryptionInstance(encrypted,aesAlg.Key,aesAlg.IV);
        }

        private string AES_DecryptStringFromBytes(byte[] cryptedText, byte[] key, byte[] IV)
        {
            string decryptedText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cryptedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decryptedText;
        }

        public ContentEntity AES_EncryptContentEntity(ContentEntity objToEncrypt, string keyString)
        {
            if (objToEncrypt.EncryptedBody == null)
            {
                byte[] key = Encoding.UTF8.GetBytes(keyString);
                AES_EncryptionInstance instance = AES_EncryptString(objToEncrypt.Body, key);
                objToEncrypt.EncryptedBody = Convert.ToBase64String(instance.encrypted);
                objToEncrypt.EncryptionKey = instance.key;
                objToEncrypt.InitialVector = instance.IV;
                return objToEncrypt;
            }
            return null;
        }
    }
}