﻿using Drink.Database;
using Drink.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Drink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {

        private readonly CocktailDatabaseContext _context;
        public IngredientController(CocktailDatabaseContext context)
        {
            _context = context;
        }


        // GET: api/<IngredientController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/<IngredientController>/5
        //[HttpGet("GetAllIngredientsByUserID")]
        //public async Task<ActionResult<IEnumerable<UserIngredient>>>GetAllIngredientsByUserID(int id)
        //{
        //    var user = await _context.UserIngredients.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return user;
        //}

        // POST api/<IngredientController>
        //[HttpPost("AddIngredient")]
        //public async Task<ActionResult<IEnumerable<Ingredient>>> AddIngredient(Ingredient ingredient)
        //{
        //    _context.Ingredients.Add(ingredient);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction("GetAllIngredientsByUserID", new { id = user.Id }, user);
        //}

        // POST api/<IngredientController>
        [HttpPut("AddIngredient")]
        public async Task<ActionResult<IEnumerable<User>>> AddIngredient(User user)
        {
       
            if (await _context.Users.FindAsync(user.Id) == null)
            {
                return NotFound();
            }
            
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAllIngredientsByUserID", new { id = user.Id }, user);
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
