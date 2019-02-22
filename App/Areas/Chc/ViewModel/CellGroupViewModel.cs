using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Chc.ViewModel
{
    public class CellGroupViewModel: Core.Models.ViewModel
    {
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Details { get; set; }
        public List<string> MeetingTimes { get; set; }
    }
}
