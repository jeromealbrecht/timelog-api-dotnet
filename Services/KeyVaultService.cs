using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace TimeLog.Services
{
    public class KeyVaultService
    {
        private readonly SecretClient _secretClient;

        public KeyVaultService(IConfiguration configuration)
        {
            var keyVaultUrl = configuration["KeyVault:Url"];
            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                throw new InvalidOperationException("KeyVault URL not configured");
            }

            _secretClient = new SecretClient(
                new Uri(keyVaultUrl),
                new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du secret {secretName}: {ex.Message}", ex);
            }
        }

        public async Task SetSecretAsync(string secretName, string secretValue)
        {
            try
            {
                await _secretClient.SetSecretAsync(secretName, secretValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la définition du secret {secretName}: {ex.Message}", ex);
            }
        }
    }
} 