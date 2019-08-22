using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Areas.Crm.ViewModels;
using Core.Extensions;

namespace App.Areas.Chc.ViewModel
{
    public class CellGroupViewModel: Core.Models.ViewModelCustomId
    {     
        public string Id { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public AddressViewModel Venue { get; set; }
        [Required]
        public string Details { get; set; }
        [MustHaveElements]
        public List<string> MeetingTimes { get; set; }
        public List<string> Tags { get; set; }

    }
}
