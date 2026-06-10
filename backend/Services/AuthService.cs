using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Dtos.Auth;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> RegisterUserAsync(RegisterAccountDto dto)
        {
            try
            {
                var appUser = new AppUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Username,
                    Email = dto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, dto.Password);

                if (!createdUser.Succeeded)
                {
                    return new AuthResultDto 
                    { 
                        Success = false, 
                        Errors = createdUser.Errors.Select(e => e.Description) 
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                
                if (!roleResult.Succeeded)
                {
                    return new AuthResultDto 
                    { 
                        Success = false, 
                        Errors = roleResult.Errors.Select(e => e.Description) 
                    };
                }

                return new AuthResultDto
                {
                    Success = true,
                    Data = new NewAccountDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    }
                };
            }
            catch (Exception e)
            {
                return new AuthResultDto { Success = false, Errors = new[] { e.Message } };
            }
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginAccountDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user == null)
                {
                    return new AuthResultDto { Success = false, Errors = new[] { "Email not found and/or incorrect password" } };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

                if (result.IsLockedOut)
                {
                    return new AuthResultDto 
                    { 
                        Success = false, 
                        Errors = new[] { "Account is locked due to multiple failed login attempts. Please try again in 5 minutes." } 
                    };
                }

                if (!result.Succeeded)
                {
                    return new AuthResultDto { Success = false, Errors = new[] { "Email not found and/or incorrect password" } };
                }

                return new AuthResultDto
                {
                    Success = true,
                    Data = new NewAccountDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                };
            }
            catch (Exception e)
            {
                return new AuthResultDto { Success = false, Errors = new[] { e.Message } };
            }
        }
    }
}