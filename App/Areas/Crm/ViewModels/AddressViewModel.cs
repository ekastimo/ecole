using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class AddressViewModel : ViewModel
    {
        public Guid ContactId { get; set; }
        public AddressCategory Category { get; set; }
        public Country Country { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string Village { get; set; }
        public string Parish { get; set; }
        public string OriginalFreeform { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public bool IsPrimary { get; set; }
    }
}