using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant4you_API.Models;

namespace Restaurant4you_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// it executes code before the creation of model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // imports the previous execution of this method
            base.OnModelCreating(modelBuilder);

            // seed the Roles data
            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Id = "u", Name = "Users", NormalizedName = "USER" },
              new IdentityRole { Id = "r", Name = "Restaurant", NormalizedName = "RESTAURANT" }
              );

        }


        // define table on the database

        public DbSet<Images> Image { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<Restaurants> Restaurant { get; set; }

    }
}