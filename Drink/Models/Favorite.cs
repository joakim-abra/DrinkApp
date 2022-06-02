using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class Favorite
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int CocktailDBFavoriteID { get; set; }

    }
}
