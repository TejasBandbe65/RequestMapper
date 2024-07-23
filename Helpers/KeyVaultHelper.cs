using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestMapper.Helpers
{
    public class KeyVaultHelper
    {
        public static string SQLConnectionString { get; set; }
        public static string BlobConnectionString { get; set; }
        public static string ServiceBusConnectionString { get; set; }

        public static void ReloadAllSecrets(IConfiguration config)
        {
            try
            {
                SQLConnectionString = config[AppSetting.SQLConnectionString];
                BlobConnectionString = config[AppSetting.BlobConnectionString];
                ServiceBusConnectionString = config[AppSetting.ServiceBusConnectionString];
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
