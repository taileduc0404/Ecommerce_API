using Ecom.API.Errors;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Infrastructure.Data;
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
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
            try
            {
                var checkEmailExist = await _userManager.FindByEmailAsync(dto.Email);
                if (checkEmailExist is not null)
                {
                    return BadRequest(new BaseCommonResponse(400, "Email mày vừa nhập đã tồn tại trong hệ thống của tao."));
                }
                var address = new Address
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Street = dto.Street,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode
                };

                // Thêm địa chỉ vào cơ sở dữ liệu
                _context.addresses.Add(address);
                await _context.SaveChangesAsync();

                var user = new ApplicationUser
                {
                    DisplayName = dto.DisplayName,
                    UserName = dto.Email,
                    Email = dto.Email,
                    Address = address
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (result.Succeeded == false)
                {
                    return BadRequest(new BaseCommonResponse(400));
                }
                return Ok(new UserDto
                {
                    DisplayName = dto.DisplayName,
                    Email = dto.Email,
                    Token = ""
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // In ra chi tiết của ngoại lệ vào console
                throw; // Ném ngoại lệ để hiển thị thông báo lỗi cho client
                //return StatusCode(500, new BaseCommonResponse(500, "Đã xảy ra lỗi đéo mong muốn, vui lòng liên hệ với tao."));
            }
        }
    }
}
