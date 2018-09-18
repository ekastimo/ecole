using System;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Company : ModelBase
    {
        [ForeignKey("Contact")]
        public new Guid Id { get; set; }
        public virtual Contact Contact { get; set; }
        public string Name { get; set; }
        public CompanyCategory Category { get; set; }
    }
}
