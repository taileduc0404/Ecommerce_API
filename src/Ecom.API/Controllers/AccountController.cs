using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ITokenService tokenService,
			IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
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
				Token = _tokenService.GenerateToken(user)
			});
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto dto)
		{
			var checkEmailExist = await _userManager.FindByEmailAsync(dto.Email);
			if (checkEmailExist is not null)
			{
				return BadRequest(new BaseCommonResponse(400, "Email mày vừa nhập đã tồn tại trong hệ thống của tao."));
			}
			var user = new ApplicationUser
			{
				DisplayName = dto.DisplayName,
				UserName = dto.Email,
				Email = dto.Email
			};

			var result = await _userManager.CreateAsync(user, dto.Password);

			if (result.Succeeded == false)
			{
				return StatusCode(400, new BaseCommonResponse(400, "Lỗi đăng ký người dùng."));
			}
			return Ok(new UserDto
			{
				DisplayName = dto.DisplayName,
				Email = dto.Email,
				Token = _tokenService.GenerateToken(user)
			});
		}

		[Authorize]
		[HttpGet]
		public ActionResult<string> TestAuthorize()
		{
			return "Hi";
		}


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetCurrentUser()
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			var user = await _userManager.FindByEmailAsync(email);
			return Ok(new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.GenerateToken(user)
			});
		}

		[HttpGet]
		public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
		{
			return Ok(await _userManager.FindByEmailAsync(email) != null);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserAddress()
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
			//return Ok(user.Address is not null ? user.Address : $"'{user.DisplayName}' Not Address");

			var result = _mapper.Map<Address, AddressDto>(user.Address);
			return Ok(result);
		}


	}
}
