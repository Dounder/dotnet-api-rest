using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTO.Common;
using AutoMapper;
using Core.Entities.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.Common;

[ApiController, Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IMapper mapper;

    public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration,
        SignInManager<IdentityUser> signInManager,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.signInManager = signInManager;
        this.mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Post(RegisterDto registerDto)
    {
        var user = new AppUser { UserName = registerDto.Username ?? registerDto.Email, Email = registerDto.Email };
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(CreateJwt(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Post(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return new BadRequestObjectResult(new { code = 400, msg = "Login error ( email / password )" });

        var result = await signInManager.PasswordSignInAsync(user.UserName!, loginDto.Password, isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
            return new BadRequestObjectResult(new { code = 400, msg = "Login error ( email / password )" });

        return Ok(CreateJwt(mapper.Map<AppUser>(user)));
    }

    [HttpGet("refresh_token")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> RefreshToken()
    {
        var id = User.FindFirstValue("id");
        var user = await userManager.FindByIdAsync(id);

        if (user is null) return new BadRequestObjectResult(new { code = 400, msg = "Invalid token" });

        return Ok(CreateJwt(mapper.Map<AppUser>(user)));
    }

    private AuthDto CreateJwt(AppUser user)
    {
        var claims = new Claim[] { new("id", user.Id) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(4);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration,
            signingCredentials: credentials);

        return new AuthDto()
        {
            User = mapper.Map<UserDto>(user),
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiration
        };
    }
}