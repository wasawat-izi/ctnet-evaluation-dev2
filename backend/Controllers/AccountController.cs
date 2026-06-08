using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ess;
using Superpower.Model;

namespace backend.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterAccountDto registerAccountDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerAccountDto.Username,
                    Email = registerAccountDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerAccountDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewAccountDto
                        {
                            Username = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                    return StatusCode(500, createdUser.Errors); 
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message); 
            }
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginAccountDto loginAccountDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(loginAccountDto.Email);

                if(user == null)
                    return Unauthorized("Email not found and/or incorrect password");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginAccountDto.Password, false);

                if(!result.Succeeded)
                    return Unauthorized("Email not found and/or incorrect password");

                return Ok(
                    new NewAccountDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}