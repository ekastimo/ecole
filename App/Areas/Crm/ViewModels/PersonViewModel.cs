using System;
using System.ComponentModel.DataAnnotations;
using App.Areas.Crm.Enums;

namespace App.Areas.Crm.ViewModels
{
    public class PersonViewModel
    {
        public Salutation? Salutation { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public CivilStatus? CivilStatus { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
    }

    
}