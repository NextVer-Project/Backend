﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NextVer.Domain.DTOs;
using NextVer.Domain.Email.Builders;
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
        private readonly IEmailService _emailService;


        public AuthController(IConfiguration configuration, IAuthRepository authRepository, IMapper mapper, IEmailService emailService)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _mapper = mapper;
            _emailService = emailService;
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

            if (registeredUser == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            var result = await SendEmail(registeredUser);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
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

            if (!user.IsVerified)
            {
                var result = await SendEmail(user);

                return result ? Unauthorized("Please confirm your e-mail address first.")
                    : StatusCode((int)HttpStatusCode.InternalServerError);
            }

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

        [HttpGet("confirmEmail")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ConfirmEmail(string token, string username)
        {
            var result = await _authRepository.ConfirmEmail(token, username);

            return result ? Ok("Email confirmed.") : Unauthorized("Verification link is not valid.");
        }

        private async Task<bool> SendEmail(User user)
        {
            var emailActionLink = Url.Action("ConfirmEmail", "Auth",
                new { Token = user.ConfirmationToken, Username = user.Username },
                ControllerContext.HttpContext.Request.Scheme);

            var message = ConfirmationEmailBuilder.BuildConfirmationMessage(user.Email, user.Username, emailActionLink);

            return await _emailService.SendEmail(message);
        }
    }
}
