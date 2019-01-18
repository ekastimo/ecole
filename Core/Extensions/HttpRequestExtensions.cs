using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string NullIpAddress = "::1";

        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                //Check if the request has a set IP address
                return connection.LocalIpAddress.IsSet()
                    //The request is local if LocalIpAddress == RemoteIpAddress
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //else request is remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }

        public static (string userId, IDictionary<string, string> userClaims) GetUser(
            this IHttpContextAccessor contextAccessor)
        {
            var user = contextAccessor.HttpContext.User;

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                ["username"] = user.Identity.Name
            };
            foreach (var userClaim in user.Claims)
            {
                data[userClaim.Type] = userClaim.Value;
            }
            return (userId: data["id"], userClaims: data);
        }
    }
}