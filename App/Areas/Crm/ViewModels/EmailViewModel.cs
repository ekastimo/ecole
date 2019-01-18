using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class EmailViewModel : MiniViewModel
    {
        public EmailCategory Category { get; set; }
        public string Address { get; set; }
        public bool IsPrimary { get; set; }
    }
}