using System;
using Core.Models;

namespace App.Areas.Events.Models
{
    public class Item : ModelBase
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatorId { get; set; }
        public string[] Images { get; set; } = { };
        public string[] Tags { get; set; } = { };
    }
}