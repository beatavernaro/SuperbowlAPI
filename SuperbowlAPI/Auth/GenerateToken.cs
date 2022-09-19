using Microsoft.IdentityModel.Tokens;
using SuperbowlAPI.Auth;
using SuperbowlAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperbowlAPI.Auth
{
    public class GenerateToken
    {
        private readonly TokenConfiguration _configuration;
        public GenerateToken(TokenConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwt(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3d3553289d6a0910cfad780140a80704f97bb4c04b068ded29b8a0e1cff9dcbb"));
            var tokenHandler = new JwtSecurityTokenHandler();

            var nameClaim = new Claim(ClaimTypes.Name, user.Username);
            var roleClaim = new Claim(ClaimTypes.Role, user.Role);
            var moduleClaim = new Claim("module", "teste");
            List<Claim> claims = new List<Claim>();
            claims.Add(nameClaim);
            claims.Add(roleClaim);
            claims.Add(moduleClaim);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_configuration.ExpirationTimeInHours),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature));

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}