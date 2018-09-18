using System.Collections.Generic;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Contact : ModelBase
    {
        public ContactCategory Category { get; set; }
        public virtual Person Person { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Identification> Identifications { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<ContactCTag> ContactTags { get; set; }
       
    }
}
