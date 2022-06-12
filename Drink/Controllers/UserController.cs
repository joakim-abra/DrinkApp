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
        public async Task<ActionResult<EditUserDTO>> CreateUser(CreateUserDTO user)
        {
            if(user.Username == null || user.Username.Length<2)
            {
                return Unauthorized("Username too short");
            }
            if (user.Password == null || user.Password.Length<2)
            {
                return Unauthorized("Password too short");
            }
            try
            {
                if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                {
                    return Unauthorized("Username must be unique");
                }
                _context.Users.Add(new User(user.Username, user.Password));
                await _context.SaveChangesAsync();
                var newUser = await _context.Users.OrderBy(x => x.Id).LastOrDefaultAsync(x => x.Id > 0);
                return CreatedAtAction("GetUserById", new { id = newUser.Id }, new EditUserDTO(newUser.Id,newUser.Username,newUser.Password));
            }
            catch(Exception)
            {
                throw;
            }
        }

        // PUT api/<UserController>/5
        [HttpPatch("EditUser")]
        public async Task<ActionResult<EditUserDTO>> EditUser(EditUserDTO user)
        {
            try
            {

            var current = await _context.Users.FindAsync(user.ID);
            if (current == null)
            {
                return NotFound();
            }
            if(await _context.Users.AnyAsync(x =>x.Username == user.Username))
                {
                    return Unauthorized("Username already in use");
                }
            else if(user.Username.Length<2 || user.Password.Length<2)
                {
                    return BadRequest("Username and password must be atleast 2 characters");
                }

            current.Username = user.Username;
            current.Password = user.Password;
            _context.Users.Update(current);

                //_context.Entry(current).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            return user;
        }



        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            return NoContent();

        }
    }
    
}
