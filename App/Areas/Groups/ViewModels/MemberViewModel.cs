using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Extensions;
using Core.Models;

namespace App.Areas.Groups.ViewModels
{
    public class MemberViewModel : ViewModel
    {
        public Guid GroupId { get; set; }
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactAvatar { get; set; }
        public Guid CreatedBy { get; set; }
        public GroupRole Role { get; set; }
        public GroupStatus Status { get; set; }
    }

    public class CreateMemberModel 
    {
        [Required]
        [ValidGuid]
        public Guid GroupId { get; set; }
        [Required]
        [ValidGuid]
        public Guid ContactId { get; set; }
        [Required]
        public GroupRole Role { get; set; }

        public Guid CreatedBy { get; set; }
    }

    public class CreateMembersModel
    {
        public Guid CreatedBy { get; set; }
        [Required]
        public Guid GroupId { get; set; }
        [Required]
        public List<Guid> ContactIds { get; set; }
        [Required]
        public GroupRole Role { get; set; }
        public GroupStatus Status { get; set; }
    }
}