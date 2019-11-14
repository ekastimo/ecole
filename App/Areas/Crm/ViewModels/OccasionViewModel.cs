using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class OccasionViewModel: MiniViewModel
    {
        public string Details { get; set; }
        public OccasionCategory Category { get; set; }
        public DateTime Value { get; set; }
    }
}
