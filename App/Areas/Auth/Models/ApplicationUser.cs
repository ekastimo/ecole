using System;
using AspNetCore.Identity.Mongo;

namespace App.Areas.Auth.Models
{
    public class ApplicationUser : MongoIdentityUser
    {
        public Guid ContactId { get; set; }
    }
}
