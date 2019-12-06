using AspNetCore.Identity.Mongo.Model;

namespace App.Areas.Auth.Models
{
    public class ApplicationRole : MongoRole
    {
        public string[] Permissions { get; set; }
        public ApplicationRole(string role) : base(role)
        {
            Permissions = new string[0];
        }
    }
}