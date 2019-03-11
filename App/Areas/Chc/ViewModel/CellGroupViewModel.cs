using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        public string Venue { get; set; }
        [Required]
        public string Details { get; set; }

        [MustHaveElements]
        public List<string> MeetingTimes { get; set; }
        
    }
}
