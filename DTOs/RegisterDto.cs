using System.ComponentModel.DataAnnotations;

namespace backendMuseum.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string FullName { get; set; } = string.Empty; // Họ và tên

        public string? Gender { get; set; } // Giới tính (tuỳ chọn)

        public string? PhoneNumber { get; set; } // Số điện thoại (tuỳ chọn)

        [Range(0, 120, ErrorMessage = "Tuổi phải từ 0 đến 120.")]
        public int? Age { get; set; } // Tuổi (tuỳ chọn)
    }
}