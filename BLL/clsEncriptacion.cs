using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public static class clsEncriptacion
    {
        // Clave fija de 32 bytes (AES-256). El IV es aleatorio en cada encriptación
        // (más seguro que uno fijo) y se guarda pegado adelante del texto encriptado,
        // así se puede recuperar al desencriptar sin tener que guardarlo aparte.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("MyTurnoSync2026SuperSecretKey32!");

        public static string Encriptar(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return textoPlano;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(textoPlano);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado)) return textoEncriptado;

            byte[] buffer = Convert.FromBase64String(textoEncriptado);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[16];
                Array.Copy(buffer, 0, iv, 0, 16);
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream(buffer, 16, buffer.Length - 16))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}