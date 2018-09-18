using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class EventViewModel:ViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public string FreeFormAddress { get; set; }
        [Required]
        public string Description { get; set; }

        public string GeoCoOrdinates { get; set; }
        public Guid CreatorId { get; set; }
        public string Image { get; set; }
        public string[] Tags { get; set; }
    }
}
