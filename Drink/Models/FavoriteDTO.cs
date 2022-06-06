using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class FavoriteDTO
    {
        public string Name { get; set; }
        public string CocktailDbID { get; set; }
        public FavoriteDTO()
        {

        }
        public FavoriteDTO(string name, string cDB)
        {
            this.Name = name;
            this.CocktailDbID = cDB;
        }
    }
}
