using tjenamannen.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace tjenamannen.Data
{
    public class ApplicationDbContext: IdentityDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		public DbSet<Word> Words { get; set; }   
        public DbSet<Player> Players { get; set; }   
        public DbSet<Ordklass> Ordklasser { get; set; }   

    }
}
