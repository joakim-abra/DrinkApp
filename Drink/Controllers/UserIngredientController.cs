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
    public class UserIngredientController : ControllerBase
    {
        private readonly CocktailDatabaseContext _context;
        public UserIngredientController(CocktailDatabaseContext context)
        {
            _context = context;
        }


        // GET: api/<UserIngredientController>
        [HttpGet("GetMyIngredients")]
        public async Task<ActionResult<UserIngredientDTO>> GetMyIngredients(int userID)
        {
            try
            {

                var user = await _context.Users.FindAsync(userID);
                if(user!=null)
                {

                    List<IngredientDTO> resp = new();
                    var ingredientList = await _context.UserIngredients.Where(x => x.UserID == userID).ToListAsync();

                    foreach (UserIngredient item in ingredientList)
                    {
                    var toAdd = await _context.Ingredients.FindAsync(item.IngredientID);
                    resp.Add(new IngredientDTO(toAdd.Name, toAdd.CocktailDBId));
                    }
                    return new UserIngredientDTO(resp);
                }
            
                return NotFound();
            }
            catch(Exception)
            {
                throw;
            }

        }

            // GET api/<UserIngredientController>/5
            [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserIngredientController>
        [HttpPost]
        public async Task<ActionResult<UserIngredientDTO>> AddUserIngredient(int userID, int ingredientID)
        {
            try
            {

            var user = await _context.Users.FindAsync(userID);
            var ingredient = await _context.Ingredients.FindAsync(ingredientID);
            if (user == null || ingredient == null)
            {
                return NotFound();
            }
            if(await _context.UserIngredients.AnyAsync(x =>x.IngredientID == ingredient.ID && x.UserID == userID))
            {
                return Unauthorized("Cannot add duplicate");
            }

            UserIngredient added = new(userID,user,ingredientID,ingredient);
            await _context.UserIngredients.AddAsync(added);
            user.Ingredients.Add(added);
            //_context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
            }
            catch(Exception)
            {
                throw;
            }

        }

        // PUT api/<UserIngredientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserIngredientController>/5
        [HttpDelete]
        public async Task<ActionResult<UserIngredientDTO>> Delete(int userID, int cocktaildbID)
        {
            try
            {
                var ingredient = await _context.UserIngredients.SingleAsync(x => x.UserID == userID && x.Ingredient.CocktailDBId == cocktaildbID);
                if(ingredient !=null)
                {
                    _context.UserIngredients.Remove(ingredient);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch(Exception)
            {
                throw;
            }
                

            }

        }
    }

