using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // allows method to infer the object type. if not there, must use [FromBody] for parameters
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(/*[FromBody]*/UserForRegisterDto userForRegisterDto)
        {
            // validate request
            /* this is not necessary because of the ApiController attribute
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            */

            userForRegisterDto.Username=userForRegisterDto.Username.ToLower();
            if(await _repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate=new User
            {
                Username=userForRegisterDto.Username
            };

            var createdUser=await _repo.Register(userToCreate,userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}