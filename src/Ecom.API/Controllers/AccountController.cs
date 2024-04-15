using Ecom.API.Errors;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
								 SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto dto)
		{
			// kiểm tra email người dùng có tồn tại không
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user is null)
			{
				// trả về Unauthorized nếu không tìm thấy email 
				return Unauthorized(new BaseCommonResponse(401, $"This Email '{dto.Email}' is not found."));
			}
			// kiểm tra password người dùng nhập vào
			var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
			if (user is null || !result.Succeeded)
			{
				// trả về Unauthorized nếu password người dùng nhập vào không đúng
				return Unauthorized(new BaseCommonResponse(401, $"This Password is not correct."));
			}

			// nếu thành công tạo và trả về thông tin người dùng
			return Ok(new UserDto
			{
				DisplayName = user.DisplayName,
				Email = dto.Email,
				Token = "con mẹ mày"
			});
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto dto)
		{
			return Ok();
		}
	}
}
