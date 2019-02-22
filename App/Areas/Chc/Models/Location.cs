﻿using Core.Models;
using System.Collections.Generic;

namespace App.Areas.Chc.Models
{
    public class Location : ModelBase
    {
        public Location()
        {
            MeetingTimes = new List<string>();
        }
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Details { get; set; }
        public List<string> MeetingTimes { get; set; }
        public List<string> HashTags { get; set; }
    }
}
