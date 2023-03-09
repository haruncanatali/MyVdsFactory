using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyVdsFactory.Domain.Addition;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Common.Managers;

public class ClaimManager
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenSettings _tokenSettings;


    public ClaimManager(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,IOptions<TokenSettings> tokenSettings)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task GenerateClaims(User appUser)
    {
        var claims = new List<Claim>();
        
        string? responseRole = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
        if (!string.IsNullOrEmpty(responseRole))
        {
            claims.Add(new Claim(ClaimTypes.Role, responseRole));
        }
        
        claims.AddRange(new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Name, appUser.UserName),
        });
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(Convert.ToInt32(_tokenSettings.TokenValidityTime)),
            IsPersistent = false
        };

        if (_httpContextAccessor.HttpContext != null)
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
    }
}