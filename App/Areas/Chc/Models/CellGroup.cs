using System;
using Core.Models;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Areas.Chc.Models
{
    public class CellGroup : ModelBaseCustomId
    {
        public CellGroup()
        {
            MeetingTimes = new List<string>();
        }

        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public Guid Location { get; set; }
        public string Details { get; set; }
        public List<string> MeetingTimes { get; set; }
    }
}
