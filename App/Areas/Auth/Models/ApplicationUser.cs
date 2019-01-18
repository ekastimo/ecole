using System;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

namespace App.Areas.Auth.Models
{
    public class ApplicationUser : MongoUser
    {
        public Guid ContactId { get; set; }
    }
}
