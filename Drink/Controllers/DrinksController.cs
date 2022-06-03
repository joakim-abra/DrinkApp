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
        
        // GET: api/<DrinksController>
        [HttpGet("GetDrinksByIngredient")]
        public async Task<ActionResult<string>> GetDrinksByIngredient(string ingredient)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={ingredient}";
            var response = await client.GetAsync(uriID);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        // GET api/<DrinksController>/5
        [HttpGet("GetDrinksByID")]
        public async Task<ActionResult<string>> GetDrinksByID(int id)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?iid={id}";
            var response = await client.GetAsync(uriID);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        [HttpGet("GetDrinksByName")]
        public async Task<ActionResult<Result>> GetDrinksByName(string name)
        {
            string uriID = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={name}";
            var response = await client.GetAsync(uriID);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Result result = JsonConvert.DeserializeObject<Result>(responseContent);
            return result;
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
