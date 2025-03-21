using Microsoft.AspNetCore.Identity;

namespace Core.Entities;
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Basket Basket { get; set; }
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }
}
