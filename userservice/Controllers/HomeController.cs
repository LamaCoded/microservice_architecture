using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userservice.data.interfaces;
using userservice.data.models;
using userservice.helper;

namespace userservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITokenRepository _userRepo;

        public UserController(ITokenRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers(int? id)
        {
            var result = _userRepo.GetUser(id);
          return Ok(DbHelper.ToJson(result));
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveUser([FromBody]
           usermodel user)
        {
            var result = _userRepo.SaveUser(user.UserName,user.Password,user.DisplayName);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
           var result=_userRepo.Login(login.UserName, login.Password);
            return Ok(result);
        }
    }
}
