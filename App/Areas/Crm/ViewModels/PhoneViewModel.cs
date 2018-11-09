using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class PhoneViewModel : MiniViewModel
    {
        public PhoneCategory Category { get; set; }
        public string Number { get; set; }
        public bool IsPrimary { get; set; }
    }
}