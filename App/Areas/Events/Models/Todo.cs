using System;
using Core.Models;

namespace App.Areas.Events.Models
{
    public class Todo : ModelBase
    {
        public Guid? EventId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public Guid CreatorId { get; set; }
        public Assignee[] Assignees { get; set; }
        public string[] Tags { get; set; }
    }

    public class Assignee
    {
        public AssigneeType Type { get; set; }
        public Guid Id { get; set; }
    }

    public enum AssigneeType
    {
        Individual = 0,
        Team = 1
    }
}