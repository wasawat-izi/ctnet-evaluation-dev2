using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;
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

        public async Task<AuthResult> RegisterUserAsync(RegisterAccountDto dto)
        {
            try
            {
                var appUser = new AppUser
                {
                    UserName = dto.Username,
                    Email = dto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, dto.Password);

                if (!createdUser.Succeeded)
                {
                    return new AuthResult 
                    { 
                        Success = false, 
                        Errors = createdUser.Errors.Select(e => e.Description) 
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                
                if (!roleResult.Succeeded)
                {
                    return new AuthResult 
                    { 
                        Success = false, 
                        Errors = roleResult.Errors.Select(e => e.Description) 
                    };
                }

                return new AuthResult
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
                return new AuthResult { Success = false, Errors = new[] { e.Message } };
            }
        }

        public async Task<AuthResult> LoginUserAsync(LoginAccountDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user == null)
                {
                    return new AuthResult { Success = false, Errors = new[] { "Email not found and/or incorrect password" } };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

                if (!result.Succeeded)
                {
                    return new AuthResult { Success = false, Errors = new[] { "Email not found and/or incorrect password" } };
                }

                return new AuthResult
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
                return new AuthResult { Success = false, Errors = new[] { e.Message } };
            }
        }
    }
}