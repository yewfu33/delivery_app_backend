using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Delivery_app.Entities;
using Delivery_app.Helpers;
using Delivery_app.Models;
using Delivery_app.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Delivery_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _iLogger;
        private readonly AppSettings _appSettings;

        public AccountsController(IAccountService accountService, IOptions<AppSettings> appSettings, ILogger iLogger)
        {
            _accountService = accountService;
            _iLogger = iLogger;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(LoginModel model)
        {
            var user = _accountService.Authenticate(model.phone_num, model.password);

            if (user == null)
                return BadRequest(new { message = "Values is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "u"),
                    new Claim(ClaimTypes.Name, user.user_id.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return user and authentication token
            return Ok(new
            {
                id = user.user_id,
                name = user.name,
                phone_num = user.phone_num,
                token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {

            Users user = new Users();
            user.name = model.name;
            user.phone_num = model.phone_num;
            user.user_type = (UserType)model.user_type;
            user.password = model.password;
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;

            try
            {
                // create user
                var createdUser = _accountService.Create(user);
                if (createdUser == null)
                {
                    return BadRequest(new { message = "Password field is incorrect" });
                }

                return Ok(new LoginModel { 
                    phone_num = model.phone_num,
                    password = model.password,
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _accountService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _accountService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _accountService.Delete(id);
            return Ok();
        }
    }
}
