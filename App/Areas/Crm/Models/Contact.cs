﻿using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Contact : ModelBase
    {
        public ContactCategory Category { get; set; }
        public  Person Person { get; set; }
        public  Company Company { get; set; }
        public MetaData MetaData { get; set; }

        public  Identification[] Identifications { get; set; }
        public  Phone[] Phones { get; set; }
        public  Email[] Emails { get; set; }
        public  Address[] Addresses { get; set; }
        public  Occasion[] Occasions { get; set; }
        public string[] Tags { get; set; }
        
    }
}
