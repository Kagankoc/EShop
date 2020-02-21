using EShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EShop.Infrastructure
{
    public class EShopContext : IdentityDbContext<AppUser>
    {
        public EShopContext(DbContextOptions<EShopContext> options) : base(options)
        {

        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }


}
