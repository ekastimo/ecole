using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Address : ModelBase
    {
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public AddressCategory Category { get; set; }
        // plot 7 Bandali Rise, Redstone house, Kampala
        public string OriginalFreeform { get; set; }
        public string LatLon { get; set; }

        public string PostalCode { get; set; }
        public Country Country { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string Parish { get; set; }
        public string Village { get; set; }
        public string Street { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPrimary { get; set; }
    }
}