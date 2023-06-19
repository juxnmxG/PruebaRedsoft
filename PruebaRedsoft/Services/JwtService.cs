using PruebaRedsoft.Models;
using System.Security.Claims;

namespace PruebaRedsoft.Services
{
    public class JwtService : IJwtService
    {
        public static IUserService _userService;

        public JwtService(IUserService userService)
         {
            _userService = userService;
         }

         public dynamic ValidateToken(ClaimsIdentity identity)
         {
             try
             {
                 if (identity.Claims.Count() == 0)
                 {
                     return new { success = false, message = "Fallo la autenticacion", result = "" };
                 }

                 var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;

                 User user;

                 if (id == _userService.Get().Id)
                 {
                     user = _userService.Get();
                     return new { success = true, message = "Se autentico correctamente", result = user };
                 }
             }
             catch (Exception ex)
             {
                 return new { success = false, message = "Error al autenticar" + ex.Message, result = "" };
             }

             return null;
         }
    }
}
