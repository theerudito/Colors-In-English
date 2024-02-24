using Microsoft.EntityFrameworkCore;

namespace ColorsInEnglish.script
{
    public partial class GameContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
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
            modelBuilder.Entity<Word>().ToTable("Words");
            modelBuilder.Entity<Word>().HasKey(w => w.IdWord);
            modelBuilder.Entity<Word>().Property(w => w.Color).IsRequired();
            modelBuilder.Entity<Word>().HasData(
                new Word { IdWord = 1, Color = "Red" },
                new Word { IdWord = 2, Color = "Blue" },
                new Word { IdWord = 3, Color = "Green" },
                new Word { IdWord = 4, Color = "Yellow" },
                new Word { IdWord = 5, Color = "Black" },
                new Word { IdWord = 6, Color = "White" },
                new Word { IdWord = 7, Color = "Purple" },
                new Word { IdWord = 8, Color = "Orange" },
                new Word { IdWord = 9, Color = "Brown" },
                new Word { IdWord = 10, Color = "Pink" },
                new Word { IdWord = 11, Color = "Gray" },
                new Word { IdWord = 12, Color = "Silver" }
            );

        }

    }
}