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
    public class IngredientController : ControllerBase
    {
        public static HttpClient client = new HttpClient();

        private readonly CocktailDatabaseContext _context;
        public IngredientController(CocktailDatabaseContext context)
        {
            _context = context;
        }


        // GET: api/<IngredientController>
        [HttpGet("GetAllIngredients")]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
        {
            try
            {
                return await _context.Ingredients.ToListAsync();
            }
            catch(Exception)
            {
                throw;
            }
        }


        [HttpGet("Ingredients/Search")]
        public async Task<ActionResult<IEnumerable<Ingredient>>> SearchIngredientByName(string name)
        {
            try
            {
               var results =  await _context.Ingredients.Where(x => x.Name.Contains(name)).ToListAsync();
               return results;
            }
            catch(Exception)
            {
                throw;
            }
        }




        //GET api/<IngredientController>/5
        [HttpGet("GetIngredientsByID")]
        public async Task<ActionResult<Ingredient>> GetIngredientsByID(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }
            return ingredient;
        }

        // POST api/<IngredientController>
        //[HttpPost("AddIngredient")]
        //public async Task<ActionResult<IEnumerable<Ingredient>>> AddIngredient(Ingredient ingredient)
        //{

        //}



        //Adds all ingredients from Cocktail database API to database

        // POST api/<IngredientController>
        [HttpGet("AddAllIngredients")]
        public async Task<ActionResult<Result>> AddAllIngredientsToDB()
        {
            try
            {
                //Check if ingredients already are added
                if(await _context.Ingredients.AnyAsync(x =>x.ID>0))
                {
                return Unauthorized();
                }

            for(int id=1;id<616;id++)
            {

                string uriID = $"http://www.thecocktaildb.com/api/json/v1/1/lookup.php?iid={id}";
                var response = await client.GetAsync(uriID);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Result result = JsonConvert.DeserializeObject<Result>(responseContent);
                if(result.ingredients!=null)
                { 
                    foreach(Ingredient ingredient in result.ingredients)
                    {
                    _context.Ingredients.Add(ingredient);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
            }
            catch(Exception)
            {
                throw;
            }
        }



        // PUT api/<IngredientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IngredientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
