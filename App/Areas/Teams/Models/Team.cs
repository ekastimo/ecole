using System;
using Core.Models;

namespace App.Areas.Teams.Models
{
    public class Team: ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
