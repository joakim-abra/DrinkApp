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
    public class DrinksController : ControllerBase
    {
        public static HttpClient client = new HttpClient();

        private readonly CocktailDatabaseContext _context;
        public DrinksController(CocktailDatabaseContext context)
        {
            _context = context;
        }

        // GET api/<DrinksController>/5
        [HttpGet("GetDrinksByID")]
        public async Task<ActionResult<Drinks>> GetDrinksByID(int id)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={id}";
            var response = await client.GetAsync(uriID);
            try
            {

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Result result = JsonConvert.DeserializeObject<Result>(responseContent);
            return result.Drinks[0];
            }
            catch(Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("GetDrinksByName")]
        public async Task<ActionResult<Result>> GetDrinksByName(string name)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={name}";
            var response = await client.GetAsync(uriID);
            try
            {

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Result result = JsonConvert.DeserializeObject<Result>(responseContent);
            return result;
            }
            catch(Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("GetDrinksByIngredientName")]
        public async Task<ActionResult<Result>> GetDrinksByIngredientName(string name)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={name}";
            var response = await client.GetAsync(uriID);
            try
            {

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Result result = JsonConvert.DeserializeObject<Result>(responseContent);
            return result;
            }
            catch(Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("GetDrinksByMyIngredients")]
        public async Task<ActionResult<Result>> GetDrinksByMyIngredients(int userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return NotFound();
            }
                List<IngredientDTO> resp = new();
                var ingredientList = await _context.UserIngredients.Where(x => x.UserID == userID).ToListAsync();

                foreach (UserIngredient item in ingredientList)
                {
                    var toAdd = await _context.Ingredients.FindAsync(item.IngredientID);
                    resp.Add(new IngredientDTO(toAdd.Name, toAdd.CocktailDBId));
                }
                
          
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={resp[0].Name}";
            var response = await client.GetAsync(uriID);
            try
            {

                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Result result = JsonConvert.DeserializeObject<Result>(responseContent);
               foreach(IngredientDTO ingredient in resp)
               {

                    string uri = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={ingredient.Name}";
                    var res = await client.GetAsync(uriID);
                    res.EnsureSuccessStatusCode();
                    string resContent = await response.Content.ReadAsStringAsync();
                    Result nextSearch = JsonConvert.DeserializeObject<Result>(responseContent);
                foreach (Drinks item in nextSearch.Drinks)
                {
                    if (result.Drinks.Contains(item))
                    {
                        result.Drinks.Remove(item);
                    }
                }

               }

                return result;
            } 
            catch(Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("RandomDrink")]
        public async Task<ActionResult<Drinks>> GetRandom()
        {
            string uriID = $"http://www.thecocktaildb.com/api/json/v1/1/random.php";
            var response = await client.GetAsync(uriID);
            try
            {
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Result randomDrink = JsonConvert.DeserializeObject<Result>(responseContent);
                return randomDrink.Drinks[0];
            }
            catch(Exception)
            {
                return NoContent();
            }
        }



        // POST api/<DrinksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DrinksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DrinksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
