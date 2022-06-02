using System;
using System.Collections.Generic;

#nullable disable

namespace Drink.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<UserFavorite> Favorites { get; set; }
        public List<UserIngredient> Ingredients { get; set; }
    }
}
