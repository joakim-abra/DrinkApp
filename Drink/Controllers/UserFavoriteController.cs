using Drink.Database;
using Drink.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserFavoriteController : ControllerBase
    {
        public static HttpClient client = new HttpClient();
        private readonly CocktailDatabaseContext _context;
        public UserFavoriteController(CocktailDatabaseContext context)
        {
            _context = context;
        }
        // GET: api/<UserFavoriteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserFavoriteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserFavoriteController>
        [HttpPost("AddFavorite")]
        public async Task<ActionResult<UserFavorite>> AddFavorite(int userID, int drinkID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return NotFound();
            }

            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkID}";
            var response = await client.GetAsync(uriID);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Result results = JsonConvert.DeserializeObject<Result>(responseContent);

            Favorite favorite = new(results.Drinks[0].strDrink, results.Drinks[0].idDrink);
                await _context.Favorites.AddAsync(favorite);
                await _context.SaveChangesAsync();

            var favorite2 = await _context.UserFavorites.OrderBy(x=>x.ID).LastOrDefaultAsync(x => x.ID > 0);
            int id;
            if (favorite2==null)
            {
                id = 1;
            }
            else
            {
                id = favorite2.ID + 1;
            }
            UserFavorite userFavorite = new(id, userID, user, favorite.ID, favorite);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            { 
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.UserFavorites ON");
            await _context.UserFavorites.AddAsync(userFavorite);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            }
            return userFavorite;

        }




        // PUT api/<UserFavoriteController>/5
        [HttpPut("AddFavoriteToUser")]
        public async Task<ActionResult<User>> AddFavoriteToUser(int userID, Favorite favorite)
        {
            User current = await _context.Users.FindAsync(userID);
            if (current == null)
            {
                return NotFound();
            }
            UserFavorite userFavorite = new(current,favorite);
            _context.UserFavorites.Add(userFavorite);
            _context.Users.Update(current);
            await _context.SaveChangesAsync();
            return current;
        }

        // DELETE api/<UserFavoriteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
