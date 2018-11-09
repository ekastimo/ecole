using Microsoft.Extensions.Configuration;

namespace Core.Extensions
{
    public static class ConfigExtentions
    {
  

        public static string GetMongoConnection(this IConfiguration configuration)
        {
            return configuration["MongoConnection:ConnectionString"];
        }

        public static string GetMongoDatabase(this IConfiguration configuration)
        {
            return configuration["MongoConnection:Database"];
        }

        public static string GetTempUploadFolder(this IConfiguration configuration)
        {
            return configuration["FileUploadLocation"];
        }

    }
}