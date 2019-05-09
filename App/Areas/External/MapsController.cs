using App.Areas.External.Models;
using AutoMapper;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.Areas.External
{
    /// <summary>
    /// Documents API
    /// </summary>
    [AreaName("Maps")]
    [Route("api/maps")]
    public class MapsController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MapsController> _logger;
        private readonly IConfiguration _configuration;

        public MapsController(IMapper mapper, ILogger<MapsController> logger,IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Searches files
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<AutoCompleteResult>))]
        public async Task<IEnumerable<AutoCompleteResult>> Search(MapSearch request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return new List<AutoCompleteResult>();
            }
            _logger.LogInformation("Query maps");
            var baseUrl = _configuration.GetMapsUrl();
            var key = _configuration.GetMapsKey();
            var url = $"{baseUrl}?key={key}&input={request.Query}&fields=geometry/location";
            
            using (var client = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                var response = await client.SendAsync(httpRequest);
                _logger.LogInformation($"processing response statusCode:{response.StatusCode} url:{url}");
                var data = await response.Content.ReadAsStringAsync();

                var structure = new
                {
                    Predictions = new List<GoogleAutoCompleteResult>()
                };

                var googleData = JsonConvert.DeserializeAnonymousType(data, structure);
                return _mapper.Map<List<AutoCompleteResult>>(googleData.Predictions);
            }
        }
    }
}
