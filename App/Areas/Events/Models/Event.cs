using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Crm.Models;
using App.Areas.Documents.Models;
using App.Areas.Events.Repositories.Event;
using Core.Models;

namespace App.Areas.Events.Models
{
    public class Event : ModelBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatorId { get; set; }
        public virtual Contact Creator { get; set; }

        public virtual ICollection<Documents> Images { get; set; }

        public string Venue { get; set; }
        public string FreeFormAddress { get; set; }
        public string GeoCoOrdinates { get; set; }
        public string Description { get; set; }

        public virtual ICollection<EventETag> EventTags { get; set; }

    }
}