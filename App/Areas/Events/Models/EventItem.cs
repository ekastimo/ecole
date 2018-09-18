using System;
using System.Collections.Generic;
using App.Areas.Crm.Models;
using App.Areas.Documents.Models;
using Core.Models;

namespace App.Areas.Events.Models
{
    public class EventItem : ModelBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatorId { get; set; }
        public virtual Contact Creator { get; set; }
        public Guid ImageId { get; set; }
        public virtual Document Image { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}