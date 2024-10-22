using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ExistsUserName(string userName)
        {
            
            if (_db.Users.FirstOrDefault(u => u.UserName == userName) == null )
            {
                return true;
            }
            return false;

        }

        public User GetUser(int userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _db.Users.OrderBy(u => u.UserName).ToList();

        }

        public Task<LoginResponseUserDto> Login(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(RegisterUserDto registerUserDto)
        {
            var encryptedPassword = getMD5(registerUserDto.Password);
            User user = new User()
            {
                UserName = registerUserDto.UserName,
                Password = encryptedPassword,
                Name = registerUserDto.UserName,
                Role = registerUserDto.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();  
            user.Password = encryptedPassword;
            return user;
        }
    }
}
