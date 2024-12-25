// DTOs cho quên mật khẩu và thay đổi mật khẩu
namespace backendMuseum.DTOs
{
    public class ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }

}