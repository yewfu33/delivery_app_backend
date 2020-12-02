using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Delivery_app.Entities;
using Delivery_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Delivery_app.Helpers;
using Microsoft.Extensions.Options;
using Delivery_app.Services;
using Microsoft.EntityFrameworkCore;

namespace Delivery_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CouriersController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;

        public CouriersController(IWebHostEnvironment webHostEnvironment, 
            AppDbContext context, IOptions<AppSettings> appSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CourierLoginModel model)
        {
            Couriers courier = await _context.couriers.FirstOrDefaultAsync(c => c.email == model.email);

            if (courier == null)
                return BadRequest(new { message = "Invalid email" });

            var onBoard = courier.onBoard;

            if (courier.onBoard == true) {
                if (courier.otp == null) return BadRequest();
                
                if (model.password.ToUpper() != courier.otp.ToUpper())
                {
                    return BadRequest(new { message = "Invalid password" });
                }
            }
            else
            {
                if (!this.Authenticate(courier, model.password)) return BadRequest(new { message = "Invalid password" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "c"),
                    new Claim(ClaimTypes.Name, courier.courier_id.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return model and authentication token
            return Ok(new
            {
                id = courier.courier_id,
                name = courier.name,
                phone_num = courier.phone_num,
                token = tokenString,
                onBoard = onBoard
            });
        } 

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CourierRegisterModel model)
        {
            try {
                Couriers courier = new Couriers();
                courier.name = model.name;
                courier.phone_num = model.phone_num;
                courier.email = model.email;
                courier.vehicle_type = (VehicleType)model.vehicle_type;
                courier.vehicle_plate_no = model.vehicle_plate_no;
                courier.created_at = DateTime.Now;
                courier.updated_at = DateTime.Now;
                // upload image to path
                courier.profile_picture = UploadedFile(model.profile_picture, model.profile_picture_name);

                // upload documents
                model.documents.ForEach((e) => {
                    courier.documents.Add(new Documents()
                    {
                        name = e.name,
                        document = uploadDocuments(e.document, e.document_name),
                    });
                });

                _context.couriers.Add(courier);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var c = await _context.couriers.FindAsync(model.id);

            if (c == null) return BadRequest(new { message = "Account no found" });

            if (!c.onBoard.GetValueOrDefault())
            {
                if (!AccountService.VerifyPasswordHash(model.oldPassword, Convert.FromBase64String(c.password), Convert.FromBase64String(c.password_salt)))
                {
                    return BadRequest(new { message = "Old Password is not correct" });
                }
            }
            else
            {
                // set onBoard to false after otp
                c.onBoard = false;
            }

            byte[] passwordHash, passwordSalt;

            AccountService.CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

            c.password = Convert.ToBase64String(passwordHash);
            c.password_salt = Convert.ToBase64String(passwordSalt);

            _context.couriers.Update(c);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public bool Authenticate(Couriers c, string password)
        {
            // check if password is correct
            if (!AccountService.VerifyPasswordHash(password, Convert.FromBase64String(c.password), Convert.FromBase64String(c.password_salt)))
                return false;

            return true;
        }

        private string UploadedFile(string base64String, string fileName)
        {
            string uniqueFileName = null;

            if (base64String != null){
                uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + fileName;
                bool ok = SaveImage(base64String, uniqueFileName);
            }

            return uniqueFileName;
        }

        public bool SaveImage(string ImgStr, string ImgName)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            String path = Path.Combine(contentRootPath, "Uploads/Profile/"); //Path

            //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            String imgPath = Path.Combine(path, ImgName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }

        private string uploadDocuments(string base64String, string fileName) {
            string uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + fileName;

            string contentRootPath = _webHostEnvironment.ContentRootPath;
            String path = Path.Combine(contentRootPath, "Uploads/Document/"); //Path

            //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            String docPath = Path.Combine(path, uniqueFileName);

            byte[] imageBytes = Convert.FromBase64String(base64String);

            System.IO.File.WriteAllBytes(docPath, imageBytes);

            return uniqueFileName;
        }
    }
}
