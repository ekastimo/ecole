using System;
using App.Areas.Teams.ViewModels;
using Core.Models;

namespace App.Areas.Teams.Models
{
    public class Team: ModelBase
    {
        public TeamPrivacy Privacy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
