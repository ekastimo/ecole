﻿using System;
using App.Areas.Crm.Enums;
using Core.Models;

namespace App.Areas.Crm.Models
{
    public class Identification : ModelBase
    {
        public IdentificationCategory Category { get; set; }
        public string Number { get; set; }
        public string IssuingCountry { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsPrimary { get; set; }
    }
}