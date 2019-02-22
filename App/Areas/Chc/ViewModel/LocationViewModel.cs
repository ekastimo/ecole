using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Chc.ViewModel
{
    public class LocationViewModel : Core.Models.ViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Venue { get; set; }
        public string Details { get; set; }
        [Required]
        public List<string> MeetingTimes { get; set; }
    }
}
