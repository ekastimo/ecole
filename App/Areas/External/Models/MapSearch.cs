using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.External.Models
{
    public class MapSearch
    {
        public string Query { get; set; }
    }

    public class GoogleAutoCompleteResult
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }

    public class AutoCompleteResult
    {
        public string Description { get; set; }   
        public string Id { get; set; }
        public string PlaceId { get; set; }
    }
}
