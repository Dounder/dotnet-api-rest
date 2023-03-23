using System.ComponentModel.DataAnnotations;

namespace API.DTO.Common;

public class AuthDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; }
}

public class LoginDto
{
    [Required, EmailAddress] public string Email { get; set; }

    [Required,
     RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{6,}$",
         ErrorMessage = "Should have one lower case letter, upper case letter, one number, one special character and minimum 6 character length")]
    public string Password { get; set; } = null!;
}

public class RegisterDto : LoginDto
{
    [Required, MinLength(2)] public string Username { get; set; } = null;
}