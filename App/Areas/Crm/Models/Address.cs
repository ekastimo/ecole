using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Address : ModelBase
    {
        #region Local Address
        public AddressCategory Category { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string Village { get; set; }
        public string Parish { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        #endregion

        #region Google Address
        public string FreeForm { get; set; }
        public string PlaceId { get; set; }
        public string LatLon { get; set; }
        #endregion

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPrimary { get; set; }   
    }
}