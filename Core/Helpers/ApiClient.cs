using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Helpers
{
    public class ApiClient
    {
        public (string, string)[] Headers;
        public string Token;
        private readonly ILogger _logger;

        public ApiClient(string token, List<(string, string)> headers, ILogger logger)
        {
            Token = token;
            _logger = logger;
            Headers = headers.ToArray();
        }

        public async Task<ApiResponse> GetAsync(string url)
        {
            _logger.LogInformation($"delete.request Url:{url}");
            using (var client = new HttpClient())
            {
                var request = PrepareRequest(url);
                request.Method = HttpMethod.Get;

                var response = await client.SendAsync(request);
                _logger.LogInformation($"processing response statusCode:{response.StatusCode} url:{url}");
                return await ProcessResponse(response);
            }
        }

        public async Task<ApiResponse> DeleteAsync(string url)
        {
            _logger.LogInformation($"delete.request url:{url}");
            using (var client = new HttpClient())
            {
                var request = PrepareRequest(url);
                request.Method = HttpMethod.Delete;

                var response = await client.SendAsync(request);
                _logger.LogInformation($"processing response statusCode:{response.StatusCode} url:{url}");
                return await ProcessResponse(response);
            }
        }

        public async Task<ApiResponse> PostAsync(string url, object obj)
        {
            _logger.LogInformation($"post.request Url:{url} Data:{obj}");
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var content = new ByteArrayContent(buffer);
                var hasMediaHeader = Headers.Any(it => it.Item1 == "Content-Type");
                if (hasMediaHeader)
                {
                    var mediaHeader = Headers.First(it => it.Item1 == "Content-Type");
                    // Do not add second header
                    content.Headers.ContentType = new MediaTypeHeaderValue(mediaHeader.Item2);
                }
                else
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var request = PrepareRequest(url);
                request.Method = HttpMethod.Post;
                request.Content = content;
                try
                {
                    var response = await client.SendAsync(request);
                    _logger.LogInformation($"processing response statusCode:{response.StatusCode} url:{url}");
                    return await ProcessResponse(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<ApiResponse> UpdateAsync(string url, object obj)
        {
            _logger.LogInformation($"update.request Url:{url} Data:{obj}");
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var content = new ByteArrayContent(buffer);

                var hasMediaHeader = Headers.Any(it => it.Item1 == "Content-Type");
                if (hasMediaHeader)
                {
                    var mediaHeader = Headers.First(it => it.Item1 == "Content-Type");
                    // Do not add second header
                    content.Headers.ContentType = new MediaTypeHeaderValue(mediaHeader.Item2);
                }
                else
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var request = PrepareRequest(url);
                request.Method = HttpMethod.Put;
                request.Content = content;

                var response = await client.SendAsync(request);
                _logger.LogInformation($"processing response statusCode:{response.StatusCode} url:{url}");
                return await ProcessResponse(response);
            }
        }


        private async Task<ApiResponse> ProcessResponse(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse
            {
                StatusCode = response.StatusCode,
                Errors = new List<string>()
            };
            var data = await response.Content.ReadAsStringAsync();
            apiResponse.Data = data; // Keep this data anyway
            if (response.IsSuccessStatusCode)
            {
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Message = "success";
            }
            // We did not get any data from the service
            else if (string.IsNullOrEmpty(data))
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        apiResponse.Message = "Access denied: refresh session";
                        apiResponse.Errors.Add("Access denied: refresh session");
                        break;
                    case HttpStatusCode.NotFound:
                        apiResponse.Message = "Invalid service path";
                        apiResponse.Errors.Add("Access denied: Login again");
                        break;
                    default:
                        apiResponse.Message = "Unknown Server Error";
                        apiResponse.Errors.Add("Unknown Server Error");
                        break;
                }
            }
            // service sent us errors
            else
            {
                //try Parsing a data validation error
                var errorStruct = new
                {
                    Message = "",
                    Errors = new List<string>(), // List of errors
                    ModelState = new Dictionary<string, string[]>()
                };

                var errorObject = JsonConvert.DeserializeAnonymousType(data, errorStruct);

                apiResponse.Message = errorObject?.Message ?? "Unknown server error";
                // It could be a validation error
                if (errorObject?.ModelState != null)
                {
                    var errors = errorObject.ModelState.Select(kvp => string.Join("\n", kvp.Value))
                        .ToList();
                    for (var i = 0; i < errors.Count; i++)
                    {
                        apiResponse.Errors.Add(errors.ElementAt(i));
                    }
                }
                // Or a list of errors
                else if (errorObject?.Errors != null)
                {
                    apiResponse.Errors.AddRange(errorObject.Errors);
                }
            }

            _logger.LogInformation(
                $"complete response statusCode:{apiResponse.StatusCode} message:{apiResponse.Message} data:{data}");
            return apiResponse;
        }

        private HttpRequestMessage PrepareRequest(string url)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url)
            };
            var hasAuthHeder = Headers.Any(it => it.Item1 == "Authorization");

            if (!hasAuthHeder)
            {
                // Do not add two auth headers
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            foreach (var header in Headers)
            {
                if (header.Item1 == "Content-Type") continue;
                request.Headers.Add(header.Item1, header.Item2);
            }

            return request;
        }
    }
}