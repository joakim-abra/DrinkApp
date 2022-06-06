using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class UserIngredientDTO
    {
        public List<IngredientDTO> UserIngredients { get; set; }

        public UserIngredientDTO()
        {

        }
        public UserIngredientDTO(List<IngredientDTO>ingredients)
        {
            this.UserIngredients = ingredients;
        }
    }
}
