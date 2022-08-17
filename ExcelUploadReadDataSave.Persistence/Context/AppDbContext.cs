using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence.Context
{
    public class AppDbContext
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Customer> Customers { get; set; }
        }
    }
}
