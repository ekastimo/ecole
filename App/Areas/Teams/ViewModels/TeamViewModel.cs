using System;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Teams.ViewModels
{
    public class TeamViewModel: ViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
