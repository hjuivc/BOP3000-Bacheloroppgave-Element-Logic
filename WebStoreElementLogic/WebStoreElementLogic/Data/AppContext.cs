using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Data
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
