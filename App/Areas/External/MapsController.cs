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

        public MapsController(IMapper mapper, ILogger<MapsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
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
            string googleKey = "AIzaSyC0hVunyllacL30Yl4weMs2YZAErtrApr0";
            string googleKey2 = "AIzaSyCoq4_-BeKtYRIs-3FjJL721G1eP5DaU0g";
            string url = $"https://maps.googleapis.com/maps/api/place/queryautocomplete/json?key={googleKey}&input={request.Query}";
            _logger.LogInformation($"delete.request Url:{url}");
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
