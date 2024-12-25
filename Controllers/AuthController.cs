using backendMuseum.DTOs;
using backendMuseum.Models;
using backendMuseum.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backendMuseum.Services;

namespace backendMuseum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IEmailSender _emailSender; // Dịch vụ gửi email

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtHelper jwtHelper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                FullName = registerDto.FullName,
                Gender = registerDto.Gender,
                Age = registerDto.Age
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // Gán vai trò mặc định là "User"
                await _userManager.AddToRoleAsync(user, "User");

                var token = _jwtHelper.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return BadRequest(new { Errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);
                if (user == null) return Unauthorized(new { Message = "User not found" });

                var token = _jwtHelper.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        // Quên mật khẩu (Gửi email thay đổi mật khẩu)

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Email không tồn tại.");

            if (string.IsNullOrEmpty(user.Email))  // Kiểm tra xem email có phải null hoặc rỗng không
                return BadRequest("Email không hợp lệ.");

            // Tạo mã token thay đổi mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            // Gửi email với liên kết thay đổi mật khẩu
            await _emailSender.SendEmailAsync(user.Email, "Yêu cầu thay đổi mật khẩu",
                $"Bạn có thể thay đổi mật khẩu của mình bằng cách nhấp vào liên kết sau: {resetLink}");

            return Ok("Một liên kết thay đổi mật khẩu đã được gửi đến email của bạn.");
        }


        // Thay đổi mật khẩu (Sau khi người dùng nhấp vào liên kết trong email)
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Email không tồn tại.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded)
                return Ok("Mật khẩu của bạn đã được thay đổi thành công.");

            return BadRequest("Thay đổi mật khẩu thất bại.");
        }
    }


}
