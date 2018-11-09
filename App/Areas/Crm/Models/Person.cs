using System;
using App.Areas.Crm.Enums;

namespace App.Areas.Crm.Models
{
    public class Person 
    {
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