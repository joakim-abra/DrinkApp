using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class UserIngredient
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
        [ForeignKey("Ingredient")]
        public int IngredientID { get; set; }
        
        public Ingredient Ingredient { get; set; }
    }
}
