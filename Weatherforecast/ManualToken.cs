using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Weatherforecast;

public class ManualToken
{
  private readonly IConfiguration config;

  public ManualToken(IConfiguration configurationCtr)
  {
    config = configurationCtr;
  }

  public string GetToken()
  {
    string secretKey = config["Jwt:Key"];
    string issuer = config["Jwt:Issuer"];
    string audience = config["Jwt:Audience"];

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
       issuer: issuer,
       audience: audience,
       claims: new[] { new Claim("role", "admin") },
       //Important: always use UTc now; Azure servers are in UTC time zone.
       expires: DateTime.UtcNow.AddHours(120),
       signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);

  }
}