using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Auth.ViewModels.Manage
{
    public class AssignRolesViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]       
        public List<string> Roles { get; set; }
        
    }
}
