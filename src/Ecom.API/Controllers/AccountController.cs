using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Services;
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
			if (CheckEmailExist(dto.Email).Result.Value)
			{
				//return BadRequest(new BaseCommonResponse(400, "Email mày vừa nhập đã tồn tại trong hệ thống của tao."));
				return new BadRequestObjectResult(new ApiValidationErrorResponse
				{
					Errors = new[]
					{
						"Email mày vừa nhập đã tồn tại trong hệ thống của tao."
					}
				});
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
			//var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			//var user = await _userManager.FindByEmailAsync(email);

			var user = await _userManager.FindEmailByClaimPrincipal(HttpContext.User);
			return Ok(new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.GenerateToken(user)
			});
		}

		[HttpGet]
		public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
		{
			var result = await _userManager.FindByEmailAsync(email);
			if(result is not null)
			{
				return true;
			}
			return false;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserAddress()
		{
			//var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			//var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
			var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);

			var result = _mapper.Map<Address, AddressDto>(user.Address);
			return Ok(result is not null ? result : $"'{user.DisplayName}' Not Address");
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> UpdateUserAddress(AddressDto dto)
		{
			var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);

			//user.Address = _mapper.Map<Address>(dto); cách ánh xạ 1, ánh xạ từ dto vào Address

			//user.Address = _mapper.Map<AddressDto, Address>(dto);// cách ánh xạ 2

			//cách ánh xạ 3
			Address address = new Address();
			user.Address = _mapper.Map(dto, address);

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				return Ok(_mapper.Map<Address, AddressDto>(user.Address));
			}
			//return Ok(dto);
			return BadRequest($"Problem while updating with {HttpContext.User}'s address");
		}
	}
}
