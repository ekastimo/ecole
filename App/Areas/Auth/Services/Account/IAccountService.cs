using System.Threading.Tasks;
using App.Areas.Auth.Models;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Auth.ViewModels.Manage;

namespace App.Areas.Auth.Services.Account
{
    public interface IAccountService
    {
        Task<ApplicationUser> Register(RegisterViewModel model);
        Task<object> AssignRoles(AssignRolesViewModel model);
    }
}