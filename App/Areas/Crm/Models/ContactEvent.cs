using App.Areas.Crm.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Crm.Models
{
    public class ContactEvent: ModelBase
    {
        public ContactEventCategory Category { get; set; }
        public DateTime Value { get; set; }
    }
}
