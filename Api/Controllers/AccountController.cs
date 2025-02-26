using Api.DTOs;
using Api.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : BaseApiController
	{
		private readonly SignInManager<AppUser> signInManager;

		public AccountController(SignInManager<AppUser> signInManager)
        {
			this.signInManager = signInManager;
		}
        [HttpPost("register")]
		public async Task<ActionResult> Register(RegisterDto registerDto)
		{
			var user = new AppUser
			{
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				UserName = registerDto.Email,
				Email = registerDto.Email
				 
			};

			var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

			if(!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return ValidationProblem();
			}
			return Ok();
		}
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> LogOut()
		{
			await signInManager.SignOutAsync();
			return NoContent();
		}

		[HttpGet("Use-Info")]
		public async Task<ActionResult> GetUserInfo()
		{
			if(User.Identity?.IsAuthenticated == false)
			{
				return NoContent();
			}

			var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
			
			return Ok( new{ 

				user.FirstName,
				user.LastName,
				user.Email,
				Address = user.Address?.ToDto(),
				Roles = User.FindFirstValue(ClaimTypes.Role)
			});
		}

		[HttpGet("auth-status")]
		public ActionResult GetAuthState()
		{
			return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
		}

		[Authorize]
		[HttpPost("address")]
		public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
		{
			var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

			if (user.Address == null)
			{
				user.Address = addressDto.ToEntity();
			}
			else
			{
				user.Address.UpdateFromDto(addressDto);
			}

			var result = await signInManager.UserManager.UpdateAsync(user);

			if (!result.Succeeded) return BadRequest("Problem updating user address");

			return Ok(user.Address.ToDto());
		}

		[Authorize]
		[HttpPost("reset-password")]
		public async Task<ActionResult> ResetPassword(string currentPassword, string newPassword)
		{
			var user = await signInManager.UserManager.GetUserByEmail(User);

			var result = await signInManager.UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

			if (result.Succeeded)
			{
				return Ok("Password updated");
			}

			return BadRequest("Failed to update password");
		}

	}
}
