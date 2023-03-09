using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Auth.Dtos;
using MyVdsFactory.Domain.Addition;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Common.Managers;

public class TokenManager
{
    private readonly TokenSettings _tokenSettings;
    private readonly UserManager<User> _userManager;

    public TokenManager(IOptions<TokenSettings> tokenSettings, UserManager<User> userManager)
    {
        _tokenSettings = tokenSettings.Value;
        _userManager = userManager;
    }

    public async Task<LoginDto> GenerateToken(User appUser)
    {
        var claims = new List<Claim>();
        
        string? responseRole = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
        if (!string.IsNullOrEmpty(responseRole))
        {
            claims.Add(new Claim(ClaimTypes.Role, responseRole));
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        DateTime tokenExpire = DateTime.Now.AddHours(_tokenSettings.TokenValidityTime);

        JwtSecurityToken token = new JwtSecurityToken(
            _tokenSettings.Issuer,
            _tokenSettings.Audience,
            claims,
            expires: tokenExpire,
            signingCredentials: credentials
        );

        LoginDto summary = new LoginDto()
        {
            Email = appUser.Email,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Role = responseRole,
        };
        
        summary.Token = new JwtSecurityTokenHandler().WriteToken(token);
        summary.TokenExpireTime = tokenExpire;
        
        DateTime refreshTokenExpire = DateTime.Now.AddHours(_tokenSettings.RefreshTokenValidityTime);
        var refreshToken = CreateRefreshToken();
        summary.RefreshToken = refreshToken;
        summary.RefreshTokenExpireTime = refreshTokenExpire;
        await _userManager.UpdateAsync(appUser);
        return summary;
    }


    private string CreateRefreshToken()
    {
        byte[] bytes = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
            return Guid.NewGuid().ToString("N") + string.Concat(bytes.Select(x => x.ToString("x2")));
        }
    }
}