using AspNetCore.Identity.Mongo;

namespace App.Areas.Auth.Models
{
    public class ApplicationRole : MongoIdentityRole
    {
       

        public ApplicationRole(string role) : base(role)
        {
        }
    }
}