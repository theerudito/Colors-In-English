using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColorsInEnglish.script
{
    public class Word
    {
        [Key]
        public int IdWord { get; set; }
        public string Color { get; set; } = string.Empty;


        public static List<Word> GetData()
        {
            return new List<Word>
            {
                new Word { IdWord = 1, Color = "Red" },
                new Word { IdWord = 2, Color = "Green" },
                new Word { IdWord = 3, Color = "Blue" },
                new Word { IdWord = 4, Color = "Aqua" },
                new Word { IdWord = 5, Color = "Yellow" },
                new Word { IdWord = 6, Color = "Purple" },
                new Word { IdWord = 7, Color = "Orange" },
                new Word { IdWord = 8, Color = "Pink" },
                new Word { IdWord = 9, Color = "Brown" },
                new Word { IdWord = 10, Color = "Magenta" },
                new Word { IdWord = 11, Color = "Lime" },
                new Word { IdWord = 12, Color = "Teal" },
                new Word { IdWord = 13, Color = "Violet" },
                new Word { IdWord = 14, Color = "Maroon" },
                new Word { IdWord = 15, Color = "Olive" },
                new Word { IdWord = 16, Color = "Gold" },
                new Word { IdWord = 17, Color = "Black" },
                new Word { IdWord = 18, Color = "Beige" },
                new Word { IdWord = 19, Color = "Salmon" },
                new Word { IdWord = 20, Color = "Coral" },
            };
        }
    }
}