using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class Favorite
    {
        public int ID { get; set; }
        // Name
        public string strDrink { get; set; }
        //ID in CocktailDB
        public string idDrink { get; set; }
        //image link
        public string strDrinkThumb { get; set; }

        public Favorite()
        {

        }
        public Favorite(string name, string dbID, string thumb)
        {
            this.strDrinkThumb = thumb;
            this.strDrink = name;
            this.idDrink = dbID;
        }
    }
}
