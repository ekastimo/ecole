using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.Areas.Auth.Models;
using App.Areas.Auth.Services.Account;
using App.Areas.Auth.ViewModels;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace App.Areas.Auth.Controllers
{
    [Route("api/auth")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public AccountController(IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAccountService accountService,
            IContactRepository contactRepository,
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _accountService = accountService;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }

            throw new NotAuthorizedException("Oops, invalid username / password");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            var user = await _accountService.Register(model);
            return await GenerateJwtToken(model.Email, user);
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
                new {c.Type, c.Value});
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet("x-claims")]
        public object MoreClaims()
        {
            return User.Claims.Select(c =>
                new {c.Type, c.Value});
        }

        private async Task<object> GenerateJwtToken(string email, ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var contact = await _contactRepository.GetByIdAsync(user.ContactId);
            var person = _mapper.Map<PersonViewModel>(contact.Person);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("id", user.Id),
                new Claim("fullName",  $"{person.FirstName} {person.MiddleName} {person.LastName}")
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            return new
            {
                Token = tokenString,
                User = person
            };
        }

        [HttpGet("users")]
        public List<UserViewModel> Get()
        {
            return _userManager.Users.Select(it => new UserViewModel
            {
                Id = it.Id,
                Email = it.Email,
                Username = it.UserName
            }).ToList();
        }

        [HttpGet("users/{id}")]
        public async Task<object> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user==null)
                throw new NotFoundException($"Invalid user: {id}");
            var roles = await _userManager.GetRolesAsync(user);
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles
            };
        }

        [HttpGet("profile")]
        public async Task<object> GetById()
        {
            var currentUser = _httpContextAccessor.GetUser();
            var id = currentUser.userId;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"Invalid user: {id}");
            var roles = await _userManager.GetRolesAsync(user);
            var name = currentUser.userClaims["fullName"];
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles,
                FullName = name
            };

        }
    }
}