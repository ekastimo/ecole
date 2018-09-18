using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Events.Models
{
    public class ETag
    {
        [Key]
        public string Id { get; set; }
        public virtual ICollection<EventETag> EventTags { get; set; }
    }


    public class EventETag
    {
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }

        public string TagId { get; set; }
        public virtual ETag Tag { get; set; }
    }
}
