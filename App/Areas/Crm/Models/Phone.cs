﻿using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Phone : ModelBase
    {
        public PhoneCategory Category { get; set; }
        public string Value { get; set; }
        public bool IsPrimary { get; set; }
    }
}