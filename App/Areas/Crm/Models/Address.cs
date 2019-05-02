using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Address : ModelBase
    {
        public Guid ContactId { get; set; }
        public AddressCategory Category { get; set; }
        
        public string Country { get; set; }
        public string District { get; set; }
        // plot 7 Bandali Rise, Redstone house, Kampala
        public string Freeform { get; set; }
        public string LatLon { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPrimary { get; set; }
    }
}