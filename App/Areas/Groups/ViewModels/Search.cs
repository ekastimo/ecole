using System;
using Core.Models;

namespace App.Areas.Groups.ViewModels
{
    public class GroupSearchRequest : SearchBase
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }
    }

    public class MemberSearchRequest : SearchBase
    {
        public Guid? GroupId { get; set; }
        public Guid? ContactId { get; set; }
        public GroupRole[] Roles { get; set; }
    }
}
