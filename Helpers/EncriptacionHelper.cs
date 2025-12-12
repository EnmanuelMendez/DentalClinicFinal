using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DentalClinic.Helpers
{
    // Helper ligero para encriptar/decriptar datos JSON con AES y también Base64.
    // Esta versión no lee configuración internamente; el secreto debe pasarse desde appsettings.
    public static class EncriptacionHelper
    {
        public static string EncodeBase64(object datos)
        {
            string json = JsonSerializer.Serialize(datos);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        public static T DecodeBase64<T>(string dataEncoded)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(dataEncoded);
                string json = Encoding.UTF8.GetString(bytes);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        public static string EncriptarAES(object datos, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 16)
                throw new ArgumentException("La clave debe tener al menos 16 caracteres", nameof(secretKey));

            string json = JsonSerializer.Serialize(datos);

            using (Aes aes = Aes.Create())
            {
                var keyBytes = new byte[32];
                var secretBytes = Encoding.UTF8.GetBytes(secretKey);
                Array.Copy(secretBytes, keyBytes, Math.Min(secretBytes.Length, keyBytes.Length));
                aes.Key = keyBytes;
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(json);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static T DesencriptarAES<T>(string dataEncriptada, string secretKey)
        {
            try
            {
                if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 16)
                    throw new ArgumentException("La clave debe tener al menos 16 caracteres", nameof(secretKey));

                byte[] buffer = Convert.FromBase64String(dataEncriptada);

                using (Aes aes = Aes.Create())
                {
                    var keyBytes = new byte[32];
                    var secretBytes = Encoding.UTF8.GetBytes(secretKey);
                    Array.Copy(secretBytes, keyBytes, Math.Min(secretBytes.Length, keyBytes.Length));
                    aes.Key = keyBytes;

                    byte[] iv = new byte[aes.BlockSize / 8];
                    Array.Copy(buffer, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string json = srDecrypt.ReadToEnd();
                        return JsonSerializer.Deserialize<T>(json);
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
