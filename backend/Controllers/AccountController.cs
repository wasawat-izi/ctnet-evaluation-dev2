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
        private readonly IAppUserService _appUserService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAuthService authService, IAppUserService appUserService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _appUserService = appUserService;
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

        [HttpPost("login/email")]
        public async Task<IActionResult> LoginByEmail([FromBody] LoginAccountByEmailDto loginAccountByEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginUserByEmailAsync(loginAccountByEmailDto);

            if (!result.Success)
                return StatusCode(500, result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("login/username")]
        public async Task<IActionResult> LoginByUsername([FromBody] LoginAccountByUsernameDto loginAccountByUsernameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginUserByUsernameAsync(loginAccountByUsernameDto);

            if (!result.Success)
                return StatusCode(500, result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("me")]
        [Authorize] 
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized();

            var result = await _appUserService.GetUserByEmail(email);
            if (!result.Success)
                return StatusCode(500, result.Errors);

            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _appUserService.GetAllUsersAsync();
            if (!result.Success)
                return StatusCode(500, result.Errors);

            return Ok(result.Data);
        }
    }
}