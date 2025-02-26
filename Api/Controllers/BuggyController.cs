﻿using Api.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
	
	public class BuggyController : ControllerBase
	{
		[HttpGet("unauthorized")]

		public IActionResult GetUnauthorized()
		{
			return Unauthorized();
		}
		[HttpGet("BadRequest")]

		public IActionResult GetBadRequest()
		{
			return BadRequest("Not A Good Request");
		}

		[HttpGet("notfound")]

		public IActionResult GetNotFound()
		{
			return NotFound();
		}

		[HttpGet("internalerror")]

		public IActionResult GetInternalError()
		{
			throw new Exception("This is a internal exception");
		}
		[HttpPost("ValidationError")]

		public IActionResult GetValidationError(CreateProductDto product)
		{
			return Ok();
		}
		[Authorize]
		[HttpGet("secret")]
		public IActionResult GetSecret()
		{
			var name = User.FindFirst(ClaimTypes.Name)?.Value;
			var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return Ok("Hello" + name + "with the id of" + id);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("admin-secret")]
		public IActionResult GetAdminSecret()
		{
			var name = User.FindFirst(ClaimTypes.Name)?.Value;
			var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var isAdmin = User.IsInRole("Admin");
			var roles = User.FindFirstValue(ClaimTypes.Role);

			return Ok(new
			{
				name,
				id,
				isAdmin,
				roles
			});
		}

	}
}
