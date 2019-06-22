using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_2_webapi.Models;
using Lab_2_webapi.Services;
using Lab_2_webapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab_2_webapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = _userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = _userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }

        [Authorize(Roles = "Admin, User_Manager")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin, User_Manager")]
        [HttpPost]
        public void Post([FromBody] UserPostModel userPostModel)
        {
            User addedBy = _userService.GetCurentUser(HttpContext);
            _userService.Create(userPostModel,addedBy);
        }

        /// <summary>
        /// Modify an user if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">id-ul user to update</param>
        /// <param name="userPostModel">obiect userPostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /users/id
        ///     {
        ///        "firstName":"First",
        ///         "lastName":"Name",
        ///         "username":"first",
        ///         "email":"first@yahoo.de",
        ///         "userRole" = "regular"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,User_Manager")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
            User addedBy = _userService.GetCurentUser(HttpContext);
            var result = _userService.Upsert(id, userPostModel, addedBy);
            return Ok(result);
        }

        /// <summary>
        /// Delete an user
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin, User_Manager")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id not fount !");
            }
            return Ok(result);
        }
    }
}