using Microsoft.IdentityModel.Tokens;
using PruebaRedsoft.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebaRedsoft.Services
{
    public class UserService : IUserService
    {
        private static User _user;
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration) {
            _user = new User { Id = "1234", Name = "JuanM", Password = "admin123" };
            _configuration = configuration;
        }

        public User Get()
        {
            return _user;
        }
        public dynamic Auth(string user, string password)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", _user.Id),
                new Claim("user", _user.Name)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, DateTime.Now.AddMinutes(60), signingCredentials: signIn);
            return new { success = true, message = "Autenticado con exito", token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    
    }
}
