using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;

namespace App.Areas.Chc.ViewModel
{
    public class CellGroupViewModel: Core.Models.ViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public string Details { get; set; }

        [MustHaveElements]
        public List<string> MeetingTimes { get; set; }
        [ValidGuid]
        public Guid Location { get; set; }
    }
}
