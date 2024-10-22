using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected ApiResponses _apiResponses;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            this._apiResponses = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsers()
        {
            var usersList = _userRepository.GetUsers();
            var usersListDto = new List<UserDto>();

            foreach (var user in usersList)
            {
                usersListDto.Add(_mapper.Map<UserDto>(user));
            }
            return Ok(usersListDto);
        }


        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {

            var userItem = _userRepository.GetUser(userId);

            if (userItem == null) { return NotFound(); }

            var userItemDto = _mapper.Map<UserDto>(userItem);

            return Ok(userItemDto);
        }



        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {

            if (!_userRepository.ExistsUserName(registerUserDto.UserName))
            {
                _apiResponses.StatusCode = HttpStatusCode.BadRequest;
                _apiResponses.IsSucces = false;
                _apiResponses.ErrorMessages.Add("Error while trying register, UserName is in use.");
                return BadRequest(_apiResponses);
            }

            if (await _userRepository.Register(registerUserDto) == null)
            {
                _apiResponses.StatusCode = HttpStatusCode.BadRequest;
                _apiResponses.IsSucces = false;
                _apiResponses.ErrorMessages.Add("Error while trying register");
                return BadRequest(_apiResponses);
            }

            _apiResponses.StatusCode = HttpStatusCode.OK;
            _apiResponses.IsSucces = true;
            return Ok(_apiResponses);

        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {

            var loginResponse = await _userRepository.Login(loginUserDto);


            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponses.StatusCode = HttpStatusCode.BadRequest;
                _apiResponses.IsSucces = false;
                _apiResponses.ErrorMessages.Add("UserName or Password is incorrect.");
                return BadRequest(_apiResponses);
            }

            _apiResponses.StatusCode = HttpStatusCode.OK;
            _apiResponses.IsSucces = true;
            _apiResponses.Result = loginResponse;
            return Ok(_apiResponses);

        }
    }
}
