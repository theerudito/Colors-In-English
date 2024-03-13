using Microsoft.EntityFrameworkCore;

namespace ColorsInEnglish.script
{
    public partial class GameContext : DbContext
    {
        public DbSet<Colors> Colors { get; set; }
        public GameContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + ConnectionDB.GetConnection());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colors>().ToTable("Colors");
            modelBuilder.Entity<Colors>().HasKey(c => c.IdColor);
            modelBuilder.Entity<Colors>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Colors>().Property(c => c.Hex).IsRequired();
            modelBuilder.Entity<Colors>().HasData(
                new Colors { IdColor = 1, Name = "YELLOW", Hex = "#FFFF00" },
                new Colors { IdColor = 2, Name = "BLUE", Hex = "#0000FF" },
                new Colors { IdColor = 3, Name = "RED", Hex = "#FF0000" });
        }
    }
}