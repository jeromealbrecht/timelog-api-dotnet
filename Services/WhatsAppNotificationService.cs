using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TimeLog.Services
{
    public interface IWhatsAppNotificationService
    {
        Task<string> SendMessageAsync(string toNumber, string contentSid, Dictionary<string, string> variables);
    }

    public class WhatsAppNotificationService : IWhatsAppNotificationService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public WhatsAppNotificationService(IConfiguration configuration)
        {
            _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? 
                         configuration["Twilio:AccountSid"] ?? 
                         throw new ArgumentNullException("TWILIO_ACCOUNT_SID");
            
            _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN") ?? 
                        throw new ArgumentNullException("TWILIO_AUTH_TOKEN");
            
            _fromNumber = Environment.GetEnvironmentVariable("TWILIO_FROM_NUMBER") ?? 
                         configuration["Twilio:FromNumber"] ?? 
                         throw new ArgumentNullException("TWILIO_FROM_NUMBER");

            _apiKey = Environment.GetEnvironmentVariable("TWILIO_API_KEY") ??
                     throw new ArgumentNullException("TWILIO_API_KEY");

            _apiSecret = Environment.GetEnvironmentVariable("TWILIO_API_SECRET") ??
                        throw new ArgumentNullException("TWILIO_API_SECRET");
            
            // Initialize Twilio client with API key and secret
            TwilioClient.Init(_apiKey, _apiSecret, _accountSid);

            Console.WriteLine("TWILIO_ACCOUNT_SID: " + _accountSid);
        }

        public async Task<string> SendMessageAsync(string toNumber, string contentSid, Dictionary<string, string> variables)
        {
            try
            {
                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber($"whatsapp:{toNumber}"))
                {
                    From = new PhoneNumber($"whatsapp:{_fromNumber}"),
                    ContentSid = contentSid,
                    ContentVariables = System.Text.Json.JsonSerializer.Serialize(variables)
                };

                var message = await MessageResource.CreateAsync(messageOptions);
                return message.Body;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send WhatsApp message: {ex.Message}", ex);
            }
        }
    }
} 