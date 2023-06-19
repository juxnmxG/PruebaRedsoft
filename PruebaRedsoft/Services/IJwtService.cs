using System.Security.Claims;

namespace PruebaRedsoft.Services
{
    public interface IJwtService
    {
        public dynamic ValidateToken(ClaimsIdentity identity);
    }
}
