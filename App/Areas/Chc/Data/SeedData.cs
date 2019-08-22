using System;
using App.Areas.Chc.Models;
using System.Collections.Generic;
using App.Areas.Crm.Models;

namespace App.Areas.Chc.Data
{
    public class SeedData
    {
        private List<string> twoSvcs = new List<string> {"9am", "11am"};
        private List<string> oneSvc = new List<string> {"10am"};

        List<Location> GetLocations()
        {
            var sampleAddress = new Address
            {
                FreeForm = "Kati Kati Restaurant, Lugogo By-Pass, Kampala, Uganda",
                Id = Guid.NewGuid(),
                PlaceId = "ChIJNyW1sJS7fRcRV1bqyGSB-yA"
            };
            return new List<Location>
            {
                new Location
                {
                    Name = "WHKatiKati", Venue = sampleAddress,
                    MeetingTimes = twoSvcs
                },
                new Location {Name = "WHBugolubi", Venue = sampleAddress, MeetingTimes = twoSvcs},
                new Location {Name = "WHNalya", Venue = sampleAddress, MeetingTimes = oneSvc}
            };
        }
    }
}