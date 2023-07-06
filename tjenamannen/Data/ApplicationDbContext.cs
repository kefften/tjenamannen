using tjenamannen.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace tjenamannen.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Word> Words { get; set; }   
        public DbSet<Player> Players { get; set; }   
        public DbSet<Ordklass> Ordklasser { get; set; }   

    }
}
