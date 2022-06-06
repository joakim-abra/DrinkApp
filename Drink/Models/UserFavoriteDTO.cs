using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class UserFavoriteDTO
    {
        public List<FavoriteDTO> Favorites { get; set; }


        public UserFavoriteDTO()
        {

        }

        public UserFavoriteDTO(List<FavoriteDTO> list)
        {
            this.Favorites = list;
        }
    }
}
