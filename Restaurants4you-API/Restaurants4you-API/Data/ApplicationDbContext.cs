using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant4you_API.Models;

namespace Restaurant4you_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // define table on the database

        public DbSet<Images> Image { get; set; }
        public DbSet<Restaurants> Restaurant { get; set; }
        public DbSet<User> Users { get; set; }

    }
}