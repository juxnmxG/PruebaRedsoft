using PruebaRedsoft.Models;

namespace PruebaRedsoft.Services
{
    public interface IUserService
    {
        User Get();
        dynamic Auth(string user, string password);
       
    }
}
