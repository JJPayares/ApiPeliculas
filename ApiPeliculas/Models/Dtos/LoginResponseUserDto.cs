namespace ApiPeliculas.Models.Dtos
{
    public class LoginResponseUserDto
    {
        public User User {  get; set; }

        public string Role { get; set; }
        public string Token { get; set; }
    }
}
