using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        #region UserController
        public UserController(IConfiguration configuration,UserRepository userRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        #endregion

        #region UserList
        [HttpGet]
        public async Task<ActionResult> UserList()
        {
            var usertList = await _userRepository.UserList();
            return Ok(usertList);
        }
        #endregion

        #region GetUserById
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUser(int userId)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        #endregion

        #region AddStudent
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            bool isInserted = await _userRepository.AddUser(user);
            if (isInserted) return Ok(new { message = "user inserted successfuly", user });
            else return BadRequest();
        }
        #endregion

        #region UpdateStudent
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            bool isUpdated = await _userRepository.UpdateUser(user);
            if (isUpdated) return Ok(new { message = "user updated successfuly", user });
            else return BadRequest();
        }
        #endregion

        #region DeleteStudent
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var isDeleted = await _userRepository.DeleteUser(userId);
            if (isDeleted) return Ok(new { message = "user deleted successfuly", userId });
            return NoContent();
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInfo login)
        {
            string email = login.Email;
            string password = login.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { message = "Email and password are required." });

            User user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound(new { message = "User Not Found" });
            if (user.Email != email || user.PasswordHash != password)
                return Unauthorized(new { message = "Invalid credentials." });

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["jwt:key"]); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("UserRole", user.Role),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["jwt:Issuer"], // Match with configuration
                Audience = _configuration["jwt:Audience"],  // Match with configuration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                UserId = user.UserId,
                UserRole = user.Role,
                Token = tokenString,
                UserName = user.FirstName+" "+user.LastName,
                Message = "Login successful"
            });
        }

        #endregion

        #region jwt
        [HttpPost("jwt")]
        public async Task<IActionResult> jwt(IFormCollection login)
        {
            string email = login["Email"];
            string password = login["Password"];

            Console.WriteLine(email + " : " + password);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Email and password are required.");
            User user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User Not Found");
            if (user.Email != email && user.PasswordHash != password)
                return Unauthorized("Invalid credentials.");

            //jwt token
            var Claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("UserId",user.UserId.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["jwt:Issuer"],
                _configuration["jwt:Audience"],
                Claim,
                signingCredentials: signIn
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine("tokenValue " + tokenValue);
            return Ok(new { token = tokenValue, message = "Login successful" });
        }
        #endregion

    }
}
