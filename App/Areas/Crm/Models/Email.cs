using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Email : ModelBase
    {
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public EmailCategory Category { get; set; }
        public string Address { get; set; }
        public bool IsPrimary { get; set; }    
    }
}
