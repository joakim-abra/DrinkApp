﻿using Drink.Database;
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
        [HttpGet("Favorites")]
        public async Task<ActionResult<UserFavoriteDTO>> GetFavorites(int id)
        {
            try
            {

            var user = await _context.Users.FindAsync(id);
           
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
                return NoContent();
            }
        }

        // POST api/<UserFavoriteController>
        [HttpPost("AddFavorite")]
        public async Task<ActionResult<UserFavoriteDTO>> AddFavorite(int userID, int drinkID)
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
            UserFavorite userFavorite = new(user, favorite);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            { 
            await _context.UserFavorites.AddAsync(userFavorite);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            }
            //string output = JsonConvert.SerializeObject(userFavorite, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            List<FavoriteDTO> resp = new();
            user = await _context.Users.FindAsync(userID);
            var favoritesList = await _context.UserFavorites.Where(x =>x.UserID==userID).ToListAsync();


            foreach(UserFavorite item in favoritesList)
            {
                var fav = await _context.Favorites.FindAsync(item.FavoriteID);
                if(item!=null)
                {
                 resp.Add(new FavoriteDTO(fav.Name,fav.CocktailDbID));
                }
            }

            return new UserFavoriteDTO(resp);

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
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
