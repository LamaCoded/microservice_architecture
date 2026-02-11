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

        // GET: api/user
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var result = _userRepo.GetUser();
          return Ok(DbHelper.ToJson(result));
        }

        // GET: api/user/search?id=1&officeId=2&userId=3&username=ram
        [HttpPost]
        public IActionResult SaveUser([FromBody]
           usermodel user)
        {
            var result = _userRepo.SaveUser(user.UserName,user.Password,user.DisplayName);
            return Ok(result);
        }
    }
}
