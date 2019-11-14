using App.Areas.Crm.Enums;
using Core.Models;
using System;

namespace App.Areas.Crm.Models
{
    public class Occasion: ModelBase
    {
        public string Details { get; set; }
        public OccasionCategory Category { get; set; }
        public DateTime Value { get; set; }
    }
}
