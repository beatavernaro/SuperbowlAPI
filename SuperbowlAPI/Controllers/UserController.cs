using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperbowlAPI.Auth;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Models;

namespace SuperbowlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly GenerateToken _generateToken;

        public UserController(IUserRepository repository, GenerateToken generateToken)
        {
            _repository = repository;
            _generateToken = generateToken;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]

        public async Task<IActionResult> Get([FromQuery] int page, int maxResults)
        {
            var users = await _repository.Get(page, maxResults);
            return Ok(users);
        }


        [HttpPost]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertUser([FromBody] UserDto userDto)
        {
            var user = await _repository.Get(userDto.Username, userDto.Password);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] Authenticate authInfo)
        {
            var user = await _repository.Get(authInfo.Username, authInfo.Password);
            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            var token = _generateToken.GenerateJwt(user);
            user.Password = "";
            return Ok(new { user = user, token = token });
        }
        
    }
}
