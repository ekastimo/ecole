using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class IdentificationViewModel : MiniViewModel
    {
        public IdentificationCategory Category { get; set; }
        public string Value { get; set; }
        public string IssuingCountry { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsPrimary { get; set; }
    }
}