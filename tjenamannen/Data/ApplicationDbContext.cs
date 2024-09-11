using tjenamannen.Models;
using Microsoft.EntityFrameworkCore;
using tjenamannen.Models.Pages;

namespace tjenamannen.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Word> Words { get; set; }   
        public DbSet<Player> Players { get; set; }   

    }
}
