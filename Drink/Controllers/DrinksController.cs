using Drink.Models;
using Microsoft.AspNetCore.Mvc;
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
