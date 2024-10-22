using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            secretKey = config.GetValue<String>("ApiSettings:Secret");
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

        public async Task<LoginResponseUserDto> Login(LoginUserDto loginUserDto)
        {
            var encryptedPassword = getMD5(loginUserDto.Password);
            var user = _db.Users.FirstOrDefault(
                                 u => u.UserName.ToLower() == encryptedPassword.ToLower());
            if (user == null) 
            {
                return new LoginResponseUserDto()
                {
                    Token = "",
                    User = null
                };   
            }

            var tokenManager = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())

                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature )
            };

            var token = tokenManager.CreateToken(tokenDescriptor);

            LoginResponseUserDto loginResponseUserDto = new LoginResponseUserDto()
            {
                Token = tokenManager.WriteToken(token),
                User = user
            };

            return loginResponseUserDto;
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

        public static string getMD5(string password)
        {
            MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = p.ComputeHash(data);
            string response = "";
            for (int i = 0; i < data.Length; i++)
                response += data[i].ToString("x2").ToLower();
            return response;

        }
    }
}
