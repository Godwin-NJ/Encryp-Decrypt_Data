using EncryptorDecryptor.Impementation.Dto;
using EncryptorDecryptor.Impementation.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using System.Text.Json;

namespace EncryptorDecryptor.Impementation
{
    public class EncryptDecryptService : IEncryptDecrypt
    {
        private readonly IConfiguration _config;
        private readonly Encrypt_DecryptConfig encrypt_decrypt_Config;
        private HttpContextAccessor _httpContext;
        public EncryptDecryptService(IConfiguration config, HttpContextAccessor httpContext)
        {
            _config = config;
            encrypt_decrypt_Config = _config.GetSection("Encryption_DecryptionSettings").Get<Encrypt_DecryptConfig>() ?? new Encrypt_DecryptConfig();
            _httpContext = httpContext;
        }
        public dynamic Decrypt(DecryptDto dto)
        {
           // decrypt encypted key 
           var rsa = RSA.Create();
            var pKeyConfig = encrypt_decrypt_Config.Rsa_PrivateKey;

            rsa.ImportFromPem(pKeyConfig);

            var decryptedKey = rsa.Decrypt(Convert.FromBase64String(dto.EncryptedAesKey), RSAEncryptionPadding.OaepSHA256);
            var aes_iv = encrypt_decrypt_Config.Aes_IV;

            //decrypt payload using decrypted key and IV
            using Aes aes = Aes.Create();
            aes.Key = decryptedKey;
            aes.IV = Convert.FromBase64String(aes_iv);

            using MemoryStream ms = new(Convert.FromBase64String(dto.EncryptedPayload));

            using CryptoStream cs = new(
               ms,
               aes.CreateDecryptor(),
               CryptoStreamMode.Read);

            using StreamReader sr = new(cs);
            var decryptedPayload = sr.ReadToEnd();

            //if (_httpContext?.HttpContext?.Response.ContentType?.Contains("text/plain") == true)
            //{
            //    return decryptedPayload;
            //}
            //_httpContext.HttpContext.Response.ContentType = "application/json";
            //var serializePayload = JsonSerializer.Serialize<dynamic>(decryptedPayload) ?? string.Empty; 


            //var dPayload = JsonSerializer.Deserialize<dynamic>(decryptedPayload) ?? string.Empty; 
            //return JsonSerializer.Deserialize<dynamic>(dPayload) ?? string.Empty; 

            if(IsJson(decryptedPayload))
            {
                return JsonSerializer.Deserialize<dynamic>(decryptedPayload) ?? string.Empty;
            }

            return decryptedPayload;
        }

        public EncryptDto Encrypt(string payload)
        {
            var encryptConfig = _config.GetSection("Encryption_DecryptionSettings").Get<Encrypt_DecryptConfig>() ?? throw new Exception("Encryption configuration is missing.");

            if (!encryptConfig.IsActive)
            {              
               throw new Exception("Encryption is not active.");
            }
          
            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(encryptConfig.Aes_Key);
            //aes.Key = Convert.FromBase64String(_config["Encryption_DecryptionSettings:Aes_Key"]);
            //_config.GetSection("Encryption_DecryptionSettings:Aes_IV").GetValue<string>("Aes_IV");            
            aes.IV = Convert.FromBase64String(encryptConfig.Aes_IV);
            using MemoryStream ms = new();

            using CryptoStream cs = new(
                ms,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write);

            using (StreamWriter sw = new(cs))
            {
                sw.Write(payload);
                sw.Close();
            }

            // Encrypted payload is now in the memory stream, convert it to a byte array and then to a base64 string for storage or transmission
            var encryptedPayload = Convert.ToBase64String(ms.ToArray());

            // AES Encryption using RSA to encrypt the AES key and IV for secure transmission
            using RSA rsa = RSA.Create();
            var Rsa_publicKey = _config["Encryption_DecryptionSettings:Rsa_PublicKey"]?.Replace("\\n", "\n");
            rsa.ImportFromPem(Rsa_publicKey);

            var aesKey = aes.Key;
            var encryptedKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
            var encrypted_AesKey = Convert.ToBase64String(encryptedKey);

            return new EncryptDto
            {
                EncryptedPayload = encryptedPayload,
                EncryptedAesKey = encrypted_AesKey,
                //AesIV = Convert.ToBase64String(aes.IV)
            };

        }

        private bool IsJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();

            return (value.StartsWith("{") && value.EndsWith("}"))
                || (value.StartsWith("[") && value.EndsWith("]"));
        }

        public string SamplePayload()
        {
            return MockData.SamplePayload;
        }

        public dynamic SamplePeopleData()
        {
            return MockData.peopleData();
        }
    }
}
