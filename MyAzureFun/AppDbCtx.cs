using Microsoft.EntityFrameworkCore;

namespace MyAzureFun
{
    public class AppDbCtx : DbContext
    {
        public AppDbCtx(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
