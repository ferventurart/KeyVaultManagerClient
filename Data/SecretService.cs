using Azure.Security.KeyVault.Secrets;
using KeyVaultManagerClient.Constants;
using LazyCache;

namespace KeyVaultManagerClient.Data;

public class SecretService
{
    private readonly SecretClient _secretClient;
    private readonly IAppCache _cache;
    private readonly IConfiguration _configuration;
    public SecretService(SecretClient secretClient, IAppCache cache, IConfiguration configuration)
    {
        _secretClient = secretClient;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<IEnumerable<SecretProperties>> GetSecretsFromCache()
    {
        return await _cache.GetAsync<IEnumerable<SecretProperties>>(KeyVaultManagerConstants.NAME_OF_SECRETS_LIST);
    }

    public string GetKeyVaultName() => $"{_configuration.GetValue<string>("KeyVault:Name")} - {KeyVaultManagerConstants.DEFAULT_TITLE}" ?? $"Key Vault - {KeyVaultManagerConstants.DEFAULT_TITLE}";

    public async Task<string> GetSecretValue(string secretName)
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
        return secret.Value;
    }
}