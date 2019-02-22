using App.Areas.Chc.Models;
using System.Collections.Generic;

namespace App.Areas.Chc.Data
{
    public class SeedData
    {
        private List<string> twoSvcs = new List<string> { "9am", "11am" };
        private List<string> oneSvc = new List<string> { "10am" };
        List<Location> GetLocations()
        {
            return new List<Location>
            {
                new Location{Name = "WHKatiKati",Venue = "Kati Kati Restaurant",MeetingTimes = twoSvcs},
                new Location{Name = "WHBugolubi",Venue = "Jazzville",MeetingTimes = twoSvcs},
                new Location{Name = "WHNalya",Venue = "Nalya",MeetingTimes = oneSvc}
            };
        }
    }
}
