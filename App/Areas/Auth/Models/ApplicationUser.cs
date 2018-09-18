using System;
using App.Areas.Crm.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
