using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

namespace App.Areas.Auth.Models
{
    public class ApplicationRole : MongoRole
    {
        public ApplicationRole(string role) : base(role)
        {
        }
    }
}