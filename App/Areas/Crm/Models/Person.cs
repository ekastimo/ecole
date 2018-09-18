using System;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Person : ModelBase
    {
        [ForeignKey("Contact")]
        public new Guid Id { get; set; }
        public virtual Contact Contact { get; set; }
        public Salutation? Salutation { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public CivilStatus? CivilStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
    }
    
}