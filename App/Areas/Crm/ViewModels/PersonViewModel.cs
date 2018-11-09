using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class PersonViewModel
    {
        public Salutation? Salutation { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CivilStatus? CivilStatus { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
    }
}