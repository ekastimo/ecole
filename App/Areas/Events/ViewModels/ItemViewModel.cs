﻿using System;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class ItemViewModel : ViewModel
    {
        public Guid EventId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Details { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string[] Images { get; set; }
        public string[] Tags { get; set; }
        public string[] Assignees { get; set; }
    }
}