using System;
using System.Collections.Generic;

namespace App.Areas.Auth.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public Guid ContactId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
