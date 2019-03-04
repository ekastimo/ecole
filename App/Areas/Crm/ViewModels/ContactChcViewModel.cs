using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;

namespace App.Areas.Crm.ViewModels
{
    public class ContactChcViewModel
    {
        [ValidGuid]
        public Guid ContactId { get; set; }

        [ValidGuid]
        public string ChurchLocation { get; set; }
        public string ChurchLocationName { get; set; }

        [ValidGuid]
        public string CellGroup { get; set; } 
        public string CellGroupName { get; set; } 
    }
}
