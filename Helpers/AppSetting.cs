using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestMapper.Helpers
{
    public static class AppSetting
    {
        public static string KeyVaultUrl { get; set; }
        public static string SecretsRefreshInterval { get; set; }
        public static string SQLConnectionString { get; set; }
        public static string BlobConnectionString { get; set; }
        public static string ServiceBusConnectionString { get; set; }

        static AppSetting()
        {
            KeyVaultUrl = Environment.GetEnvironmentVariable("KeyVaultUrl") ?? "Not Found";
            SecretsRefreshInterval = Environment.GetEnvironmentVariable("SecretsRefreshInterval") ?? "Not Found";
            SQLConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString") ?? "Not Found";
            BlobConnectionString = Environment.GetEnvironmentVariable("BlobConnectionString") ?? "Not Found";
            ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? "Not Found";
        }
    }
}
