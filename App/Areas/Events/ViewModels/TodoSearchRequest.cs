using System;
using Core.Models;

namespace App.Areas.Events.ViewModels
{
    public class TodoSearchRequest : SearchBase
    {
        public Guid? Id { get; set; }
        public Guid? Assignee { get; set; }
        
    }
}