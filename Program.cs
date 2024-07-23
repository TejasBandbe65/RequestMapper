using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RequestMapper.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, config) =>
    {
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var keyVaultClient = new KeyVaultClient(
            new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

        int timeinterval_min = Convert.ToInt32(AppSetting.SecretsRefreshInterval);

        var options = new AzureKeyVaultConfigurationOptions()
        {
            ReloadInterval = TimeSpan.FromMinutes(timeinterval_min),
            Vault = AppSetting.KeyVaultUrl,
            Client = keyVaultClient
        };

        config.AddAzureKeyVault(options);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        services.AddSingleton<IConfiguration>(configuration);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 104857600;
        });
    })
    .Build();

        host.Run();
    }
}