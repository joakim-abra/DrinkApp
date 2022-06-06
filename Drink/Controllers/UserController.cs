using Drink.Database;
using Drink.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly CocktailDatabaseContext _context;
        public UserController(CocktailDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
  

        // POST api/<UserController>
        [HttpPost("CreateUser")]
        public async Task<ActionResult<IEnumerable<EditUserDTO>>> CreateUser(CreateUserDTO user)
        {
            _context.Users.Add(new User(user.Username, user.Password));
            await _context.SaveChangesAsync();
            var newUser = await _context.Users.OrderBy(x => x.Id).LastOrDefaultAsync(x => x.Id > 0);
            //newUser.Username = user.Username;
            //newUser.Password = user.Password;
            //_context.Users.Update(newUser);
            return CreatedAtAction("GetUserById", new { id = newUser.Id }, user);
        }

        // PUT api/<UserController>/5
        [HttpPut("EditUser")]
        public async Task<ActionResult<EditUserDTO>> EditUser(EditUserDTO user)
        {
            var current = await _context.Users.FindAsync(user.ID);
            if (current == null)
            {
                return NotFound();
            }
            current.Username = user.Username;
            current.Password = user.Password;
            _context.Users.Update(current);
            await _context.SaveChangesAsync();
            return user;
        }


        //[Route("api/{userid}/")]
        //[HttpPatch("{AddFavorite}")]
        //public async Task<ActionResult<User>> Patch(User user)
        //{
        //    var current = await _context.Users.FindAsync(user.Id);
        //    if (current==null)
        //    {
        //        return NotFound();
        //    }
        //    return user;
        //}



        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
    
}
