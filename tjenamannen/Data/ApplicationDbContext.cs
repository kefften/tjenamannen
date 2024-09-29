using tjenamannen.Models;
using Microsoft.EntityFrameworkCore;
using tjenamannen.Models.Pages;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace tjenamannen.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Word> Words { get; set; }   
        public DbSet<Player> Players { get; set; }   
        public DbSet<Player> ApplicationUser { get; set; }   

    }
}
