using Azure.Security.KeyVault.Secrets;
using KeyVaultManagerClient.Constants;
using LazyCache;
using System.Collections.Immutable;

namespace KeyVaultManagerClient.Extensions;

public static class SecretInitExtension
{
    public static void LoadSecretsFromVault(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var secretClient = scope.ServiceProvider.GetRequiredService<SecretClient>();
        var cache = scope.ServiceProvider.GetRequiredService<IAppCache>();

        IEnumerable<SecretProperties> secrets = secretClient.GetPropertiesOfSecrets();

        cache.Add(KeyVaultManagerConstants.NAME_OF_SECRETS_LIST, secrets.ToImmutableList());
    }
}
