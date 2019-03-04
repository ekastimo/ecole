using Core.Models;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Areas.Chc.Models
{
    public class Location : ModelBaseCustomId
    {
        public Location()
        {
            MeetingTimes = new List<string>();
        }
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Details { get; set; }
        public List<string> MeetingTimes { get; set; }
        public List<string> HashTags { get; set; }
    }
}
