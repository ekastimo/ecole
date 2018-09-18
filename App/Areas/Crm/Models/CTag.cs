using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Crm.Models
{
    public class CTag
    {
        [Key]
        public string Id { get; set; }
        public virtual ICollection<ContactCTag> ContactTags { get; set; }
    }

    public class ContactCTag
    {
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string TagId { get; set; }
        public virtual CTag Tag { get; set; }
    }
}
