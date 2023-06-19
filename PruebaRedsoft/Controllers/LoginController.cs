using Microsoft.AspNetCore.Mvc;
using PruebaRedsoft.Services;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace PruebaRedsoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService userService;
        public LoginController(IUserService userService) {
            this.userService = userService;
        }
        [HttpPost]
        public dynamic Login([FromBody] Object dataUser) {
            var data = JsonConvert.DeserializeObject<dynamic>(dataUser.ToString());

            string user = data.name.ToString();
            string password = data.password.ToString();

            if (userService.Get().Name != user || userService.Get().Password != password)
            {
                return NotFound("Usuario incorrecto");
            }

            dynamic result = userService.Auth(user, password);
            return result;
        }
    }
}
