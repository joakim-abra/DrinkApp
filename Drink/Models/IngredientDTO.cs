using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class IngredientDTO
    {
        public string Name { get; set; }
        public int CocktailDBid { get; set; }

        public IngredientDTO()
        {

        }
        public IngredientDTO(string name, int id)
        {
            this.Name = name;
            this.CocktailDBid = id;
        }
    }
}
