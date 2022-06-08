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

        // GET api/<UserFavoriteController>/5
        [HttpGet("Favorites")]
        public async Task<ActionResult<UserFavoriteDTO>> GetFavorites(int id)
        {
            try
            {

            var user = await _context.Users.FindAsync(id);
                if(user ==null)
                {
                    return NotFound();
                }

                List<FavoriteDTO> resp = new();
                var favoritesList = await _context.UserFavorites.Where(x => x.UserID == id).ToListAsync();
                foreach (UserFavorite item in favoritesList)
                {
                    var fav = await _context.Favorites.FindAsync(item.FavoriteID);
                    if (item != null)
                    {
                        resp.Add(new FavoriteDTO(fav.Name, fav.CocktailDbID));
                    }
                }
                return new UserFavoriteDTO(resp);
            }
            catch(Exception)
            {
                throw;
            }
        }

        // POST api/<UserFavoriteController>
        [HttpPost("AddFavorite")]
        public async Task<ActionResult<UserFavoriteDTO>> AddFavorite(int userID, int drinkID)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkID}";
            try
            {

            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var response = await client.GetAsync(uriID);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
                
                Result results = JsonConvert.DeserializeObject<Result>(responseContent);
                if(results.Drinks==null)
                {
                    return NotFound("Invalid drink id");
                }

                    Favorite favorite = new(results.Drinks[0].strDrink, results.Drinks[0].idDrink);
                    UserFavorite userFavorite = new(user, favorite);
                    await _context.UserFavorites.AddAsync(userFavorite);
                    await _context.SaveChangesAsync();

                ////Return user's favorites
                //List<FavoriteDTO> resp = new();
                //user = await _context.Users.FindAsync(userID);
                //var favoritesList = await _context.UserFavorites.Where(x => x.UserID == userID).ToListAsync();

                //foreach (UserFavorite item in favoritesList)
                //{
                //    var fav = await _context.Favorites.FindAsync(item.FavoriteID);
                //    if (item != null)
                //    {
                //        resp.Add(new FavoriteDTO(fav.Name, fav.CocktailDbID));
                //    }
                //}
                //return new UserFavoriteDTO(resp);

                //Change to Created Response without returning an object how??
                return Ok();

            }
            catch(Exception)
            {
                throw;
            }
        }

        //// PUT api/<UserFavoriteController>/5
        //[HttpPut("AddFavoriteToUser")]
        //public async Task<ActionResult<User>> AddFavoriteToUser(int userID, Favorite favorite)
        //{
        //    User current = await _context.Users.FindAsync(userID);
        //    if (current == null)
        //    {
        //        return NotFound();
        //    }
        //    UserFavorite userFavorite = new(current,favorite);
        //    _context.UserFavorites.Add(userFavorite);
        //    _context.Users.Update(current);
        //    await _context.SaveChangesAsync();
        //    return current;
        //}

        // DELETE api/<UserFavoriteController>/5
        [HttpDelete("Delete")]
        public async Task<ActionResult<UserFavorite>> DeleteFavorite(int userID, int CocktailDbID)
        {
            try
            {
            var userFavorite = await _context.UserFavorites.SingleOrDefaultAsync(x => x.UserID == userID && x.Favorite.CocktailDbID == Convert.ToString(CocktailDbID));
            if(userFavorite == null)
                {
                    return NotFound();
                }
            _context.UserFavorites.Remove(userFavorite);
            await _context.SaveChangesAsync();
            return NoContent();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
