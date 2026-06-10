using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;     
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAccountDto registerAccountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(registerAccountDto);

            if (!result.Success)
                return StatusCode(500, result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("login")]
        // [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginAccountDto loginAccountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginUserAsync(loginAccountDto);

            if (!result.Success)
                return Unauthorized(result.Errors.FirstOrDefault());

            return Ok(result.Data);
        }

        [HttpGet("me")]
        [Authorize] 
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (email == null) return Unauthorized();

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return NotFound("User not found");

                return Ok(new 
                {
                    Username = user.UserName,
                    Email = user.Email
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userList = users.Select(u => new 
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                });

                return Ok(userList);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}