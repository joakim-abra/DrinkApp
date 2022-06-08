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

        // POST api/<UserIngredientController>
        [HttpPost("AddUserIngredient")]
        public async Task<ActionResult<UserIngredientDTO>> AddUserIngredient(int userID, int ingredientID)
        {
            var user = await _context.Users.FindAsync(userID);
            var ingredient = await _context.Ingredients.FindAsync(ingredientID);
            if (user == null || ingredient == null)
            {
                return NotFound();
            }
            UserIngredient added = new(userID,user,ingredientID,ingredient);
            await _context.UserIngredients.AddAsync(added);
            user.Ingredients.Add(added);
            //_context.Users.Update(user);
            await _context.SaveChangesAsync();

            //RETURNS ALL = REDUNDANT
            //var myIngredients =  _context.UserIngredients.Where(x => x.UserID == userID).ToList();
            //List < IngredientDTO > resp= new();
           
            //foreach(UserIngredient item in myIngredients)
            //{
            //    var toAdd = await _context.Ingredients.FindAsync(item.IngredientID);
            //    resp.Add(new IngredientDTO(toAdd.Name, toAdd.CocktailDBId));
                
            //}
            //return new UserIngredientDTO(resp);

            //RETURN OK FOR NOW.
            return Ok();

        }

        // DELETE api/<UserIngredientController>/5
        [HttpDelete("DeleteIngredient")]
        public async Task<ActionResult<UserIngredientDTO>> Delete(int userID, int cocktaildbID)
        {
            try
            {

            var ingredient = await _context.UserIngredients.SingleOrDefaultAsync(x => x.UserID == userID && x.Ingredient.CocktailDBId == cocktaildbID);
            if(ingredient !=null)
            {
                _context.UserIngredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }
            //RETURNS ALL INGREDIENTS = REDUNDANT.
            //var myIngredients = _context.UserIngredients.Where(x => x.UserID == userID).ToList();
            //List<IngredientDTO> resp = new();

            //foreach (UserIngredient item in myIngredients)
            //{
            //    var toAdd = await _context.Ingredients.FindAsync(item.IngredientID);
            //    resp.Add(new IngredientDTO(toAdd.Name, toAdd.CocktailDBId));

            //}
            //return new UserIngredientDTO(resp);
 
                return NoContent();
                

            }
            catch(Exception)
            {
                throw;
            }

        }
    }
}
