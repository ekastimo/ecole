using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class EventItemViewModel: ViewModel
    {
        public Guid EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
      
        public Guid? CreatorId { get; set; }
        public Guid? ImageId { get; set; }
        public ICollection<Guid> Documents { get; set; }
    }
}
