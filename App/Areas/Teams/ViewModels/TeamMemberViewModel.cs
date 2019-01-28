using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Teams.ViewModels
{
    public class TeamMemberViewModel : ViewModel
    {
        public Guid TeamId { get; set; }
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactAvatar { get; set; }
        public string CreatedBy { get; set; }
        public TeamRole Role { get; set; }
        public TeamStatus Status { get; set; }
    }

    public class TeamMemberMultipleViewModel 
    {
        public string CreatedBy { get; set; }
        [Required]
        public Guid TeamId { get; set; }
        [Required]
        public List<Guid> ContactIds { get; set; }
        [Required]
        public TeamRole Role { get; set; }
        public TeamStatus Status { get; set; }
    }
}