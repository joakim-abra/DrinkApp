using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        public int IdIngredient { get; set; }

        public Ingredient(int ID)
        {
            this.IdIngredient = ID;
        }
    }


}
