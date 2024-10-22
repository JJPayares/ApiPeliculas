using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int userId);

        bool ExistsUserName(string userName);

        Task<LoginResponseUserDto> Login(LoginUserDto loginUserDto);

        Task<User> Register(RegisterUserDto registerUserDto);


    }
}
