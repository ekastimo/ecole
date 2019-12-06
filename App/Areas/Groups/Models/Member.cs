using System;
using App.Areas.Groups.ViewModels;
using Core.Models;

namespace App.Areas.Groups.Models
{
    public class Member: ModelBase
    {
        public Guid GroupId { get; set; }
        public Guid ContactId { get; set; }
        public Guid CreatedBy { get; set; }
        public GroupRole Role { get; set; }
        public GroupStatus Status { get; set; }
    }
}