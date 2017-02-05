using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Utils
{
    public static class Crypto
    {
        private const string chaveCifragem = "ATXVS2017XYW";
        public static string md5encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(frase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha1encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(frase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha256encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(frase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha384encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA384Managed sha384hasher = new SHA384Managed();
            byte[] hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(frase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha512encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512hasher = new SHA512Managed();
            byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(frase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }

        public static string Codificar(string textoPuro)
        {
            //string chaveCifragem = "ATXVS2017XYW";
            byte[] bytesLimpos = Encoding.Unicode.GetBytes(textoPuro);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(chaveCifragem, new byte[]
                { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesLimpos, 0, bytesLimpos.Length);
                        cs.Close();
                    }
                    textoPuro = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textoPuro;
        }

        public static string Decodificar(string textoCifrado)
        {
            //string chaveCifragem = "MACVS2014XYW";
            byte[] bytesCifrados = Convert.FromBase64String(textoCifrado);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(chaveCifragem, new byte[]
                { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesCifrados, 0, bytesCifrados.Length);
                        cs.Close();
                    }
                    textoCifrado = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return textoCifrado;
        }
    }
}
