using Newtonsoft.Json;

namespace App.Areas.External.Models
{
    public class MapSearch
    {
        public string Query { get; set; }
    }

    public class GoogleAutoCompleteResult
    {
        [JsonProperty("description")]
        public string FreeForm { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }

    public class AutoCompleteResult
    {
        public string FreeForm { get; set; } 
        public string PlaceId { get; set; }
        public override string ToString()
        {
            return base.ToString(); 
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
