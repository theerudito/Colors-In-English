using Microsoft.EntityFrameworkCore;

namespace ColorsInEnglish.script
{
    public partial class GameContext : DbContext
    {
        public DbSet<Naruto> Naruto { get; set; }
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
            modelBuilder.Entity<Naruto>().ToTable("Characters");
            modelBuilder.Entity<Naruto>().HasKey(c => c.IdCharacter);
            modelBuilder.Entity<Naruto>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Naruto>().Property(c => c.Clan).IsRequired();
            modelBuilder.Entity<Naruto>().Property(c => c.Age).IsRequired();
            modelBuilder.Entity<Naruto>().Property(c => c.Avatar).IsRequired();
            modelBuilder.Entity<Naruto>().HasData(
                new Naruto { IdCharacter = 1, Name = "NARUTO", Clan = "UZUMAKI", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img1&radius=50&backgroundColor=d1d4f9" },
                new Naruto { IdCharacter = 2, Name = "HINATA", Clan = "HYUGA", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img2&radius=50&backgroundColor=d1d4f9" },
                new Naruto { IdCharacter = 3, Name = "SASUKE", Clan = "UCHIHA", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img3&radius=50&backgroundColor=d1d4f9" },
                new Naruto { IdCharacter = 4, Name = "SAKURA", Clan = "HARUNO", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img4&radius=50&backgroundColor=d1d4f9" }
            );
        }
    }
}
