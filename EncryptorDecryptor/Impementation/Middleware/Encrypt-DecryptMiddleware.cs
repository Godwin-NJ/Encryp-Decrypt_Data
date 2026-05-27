using EncryptorDecryptor.Impementation.Dto;
using System.Text.Json;
//using Microsoft.AspNetCore.Http;

namespace EncryptorDecryptor.Impementation.Middleware
{
    public class Encrypt_DecryptMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly Encrypt_DecryptConfig ED_Config;      

        public Encrypt_DecryptMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;          
            ED_Config = _config.GetSection("Encryption_DecryptionSettings").Get<Encrypt_DecryptConfig>() ?? new Encrypt_DecryptConfig();
        }

        public async Task InvokeAsync(HttpContext context, IEncryptDecrypt _encryptDecryptService)
        {
            // Here you can add logic to encrypt the request body before it reaches the controller
            // and decrypt the response body before it is sent back to the client.
            // For example, you can read the request body, encrypt it, and then replace the original body with the encrypted version.
            // Similarly, you can capture the response body, decrypt it, and then send the decrypted version back to the client.
            //for get request encrypt response and for post request decrypt request body and encrypt response body

             if (ED_Config.IsActive && context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase) 
                && ED_Config.Apis != null 
                && ED_Config.Apis.Contains(context.Request.Path))
             {
               
                    var originalBodyStream = context.Response.Body;
                    using var memoryStream = new MemoryStream();
                    context.Response.Body = memoryStream;

                    await _next(context);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(memoryStream)
                                       .ReadToEndAsync();

                    if(string.IsNullOrEmpty(responseBody))
                    {
                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsync(string.Empty);
                        return;
                    }

                    var encryptedResponse = _encryptDecryptService.Encrypt(responseBody);

                    var serializedEncryptedResponse = JsonSerializer.Serialize(encryptedResponse);

                    context.Response.ContentType = "application/json";

                    context.Response.Body = originalBodyStream;

                    await context.Response.WriteAsync(serializedEncryptedResponse);

                    return;           
           
              
            }
            else if (ED_Config.IsActive && context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                  && ED_Config.Apis != null
                  && ED_Config.Apis.Contains(context.Request.Path))                
            {
                // Logic to decrypt the request body and encrypt the response body for POST requests
                await _next(context);

                //return;
            }

            await _next(context);
        }

    }
}
