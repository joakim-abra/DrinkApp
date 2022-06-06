using Newtonsoft.Json;
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

        public int CocktailDBId { get; set; }

        public string Name { get; set; }

    }


}
