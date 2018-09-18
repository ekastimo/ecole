using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class EmailViewModel : ViewModel
    {
        public Guid ContactId { get; set; }
        public EmailCategory Category { get; set; }
        public string Address { get; set; }
        public bool IsPrimary { get; set; }
    }
}