using Microsoft.Extensions.Configuration;

namespace Core.Extensions
{
    public static class ConfigExtentions
    {
        public static string GetLoggingServer(this IConfiguration configuration)
        {
            return configuration["Servers:Log"];
        }
        public static string GetAuthServer(this IConfiguration configuration)
        {
            return configuration["Servers:Authentication"];
        }

        public static string GetClientId(this IConfiguration configuration)
        {
            return configuration["ClientId"];
        }
        public static string GetClientSecret(this IConfiguration configuration)
        {
            return configuration["ClientSecret"];
        }

    }
}