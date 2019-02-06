using System;
using Core.Models;

namespace App.Areas.Events.Models
{
    public class Event : ModelBase
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string Venue { get; set; }
        public string FreeFormAddress { get; set; }
        public string GeoCoOrdinates { get; set; }
        public string[] Images { get; set; } = { };
        public string[] Tags { get; set; } = { };
        public Item[] Items { get; set; } = { };
    }
}