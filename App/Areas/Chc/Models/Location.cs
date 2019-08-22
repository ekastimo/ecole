using Core.Models;
using System.Collections.Generic;
using App.Areas.Crm.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Areas.Chc.Models
{
    public class Location : ModelBaseCustom
    {
        public Location()
        {
            MeetingTimes = new List<string>();
        }
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Venue { get; set; }
        public string Details { get; set; }
        public List<string> MeetingTimes { get; set; }
        public List<string> Tags { get; set; }
    }
}
