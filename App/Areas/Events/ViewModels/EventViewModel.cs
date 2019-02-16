using System;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class EventViewModel : ViewModel
    {
        [Required] public string Name { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public string Venue { get; set; }
        [Required] public string FreeFormAddress { get; set; }
        [Required] public string Details { get; set; }

        public string GeoCoOrdinates { get; set; }
        public Guid CreatedBy { get; set; }
        public string[] Images { get; set; } = { };
        public string[] Tags { get; set; } = { };
        public ItemViewModel[] Items { get; set; } = { };
    }
}