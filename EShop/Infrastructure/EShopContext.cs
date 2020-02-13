using EShop.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure
{
    public class EShopContext : DbContext
    {
        public EShopContext(DbContextOptions<EShopContext> options) : base(options)
        {

        }

        public DbSet<Page> Pages { get; set; }
    }


}
