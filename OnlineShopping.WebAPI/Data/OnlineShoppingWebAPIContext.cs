using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Core;

namespace OnlineShopping.WebAPI.Data
{
    public class OnlineShoppingWebAPIContext : DbContext
    {
        public OnlineShoppingWebAPIContext (DbContextOptions<OnlineShoppingWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineShopping.Core.Category> Category { get; set; } = default!;
        public DbSet<OnlineShopping.Core.Product> Product { get; set; } = default!;
    }
}
