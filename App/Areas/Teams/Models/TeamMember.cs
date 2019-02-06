using System;
using App.Areas.Teams.ViewModels;
using Core.Models;

namespace App.Areas.Teams.Models
{
    public class TeamMember: ModelBase
    {
        public Guid TeamId { get; set; }
        public Guid ContactId { get; set; }
        public Guid CreatedBy { get; set; }
        public TeamRole Role { get; set; }
        public TeamStatus Status { get; set; }
    }
}