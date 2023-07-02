using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NextVerBackend.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;


        public AuthController(IConfiguration configuration, IAuthRepository authRepository, IMapper mapper)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            if (await _authRepository.UserExists(userForRegister.Username))
                return BadRequest("A user with this name already exists.");

            if (await _authRepository.EmailExists(userForRegister.Email))
                return BadRequest("This email address is already being used by another user.");

            var user = _mapper.Map<User>(userForRegister);

            var registeredUser = await _authRepository.Register(user, userForRegister.Password);

            return registeredUser == null ? StatusCode((int)HttpStatusCode.InternalServerError) : Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _authRepository.Login(userForLogin.Username, userForLogin.Password);

            if (user == null)
                return Unauthorized();

            var claims = new[]
                        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key;

            try
            {
                key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(
                new
                {
                    token = tokenHandler.WriteToken(token)
                });
        }
    }
}
