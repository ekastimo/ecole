using System;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class EventSearchRequest:SearchBase
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }
    }
    
}
