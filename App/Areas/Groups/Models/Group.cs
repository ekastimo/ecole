using System;
using App.Areas.Groups.ViewModels;
using Core.Models;

namespace App.Areas.Groups.Models
{
    public class Group : ModelBase
    {
        public GroupPrivacy Privacy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? Parent { get; set; }
        public string Tag { get; set; }
    }
}