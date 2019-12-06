using System;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace App.Areas.Groups.ViewModels
{
    public class GroupViewModel: ViewModel
    {
        [Required]
        public GroupPrivacy Privacy { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? Parent { get; set; }
        [Required]
        public string Tag { get; set; }
    }
}
