using System;
using Core.Models;

namespace App.Areas.Teams.ViewModels
{
    public class TeamSearchRequest : SearchBase
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }
    }

    public class TeamMemberSearchRequest : SearchBase
    {
        public Guid? TeamId { get; set; }
        public Guid? ContactId { get; set; }
        public TeamRole[] Roles { get; set; }
    }
}
