using App.Areas.Crm.Enums;
using Core.Models;
using System;

namespace App.Areas.Crm.ViewModels
{
    public class AddressViewModel : MiniViewModel
    {
        public Guid ContactId { get; set; }
        public AddressCategory Category { get; set; }

        public string Country { get; set; }
        public string District { get; set; }
        public string Freeform { get; set; }
        public string LatLon { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPrimary { get; set; }
    }
}