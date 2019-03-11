using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Extensions;

namespace App.Areas.Chc.ViewModel
{
    public class LocationViewModel : Core.Models.ViewModelCustomId
    {

        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Venue { get; set; }
        public string Details { get; set; }
        [MustHaveElements]
        public List<string> MeetingTimes { get; set; }
    }
}
