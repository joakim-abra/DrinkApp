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
    [Route("api/login")]
    [ApiController]
    [Produces("application/json")]
    public class LogInController : ControllerBase
    {
        private readonly CocktailDatabaseContext _context;
        public LogInController(CocktailDatabaseContext context)
        {
            _context = context;
        }

        // POST api/<LogInController>
        [HttpPost]
        public async Task<ActionResult<EditUserDTO>> LogIn([FromBody] LogIn login)
        {
            var found = await _context.Users.AnyAsync(x => x.Username == login.Username && x.Password == login.Password);
            if(found)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x =>x.Username == login.Username && x.Password == login.Password);
                EditUserDTO ret = new();
                ret.ID = user.Id;
                ret.Password = user.Password;
                ret.Username = user.Username;
                return ret;

            }
            //if (await _context.Users.AnyAsync(x => x.Username == user.Username && x.Password == user.Password))
            //{
            //    return user;
            //}
            return  Unauthorized();
        }

    }
}
