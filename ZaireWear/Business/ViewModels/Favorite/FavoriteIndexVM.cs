using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Favorite
{
    public class FavoritesIndexVM
    {
        public List<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
    }
}
