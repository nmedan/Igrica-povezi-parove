using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace MemoryGameExample.Model
{
    public class HighscoresContext : DbContext
    {
        public DbSet<Highscores> Highscores { get; set; }
       

        public HighscoresContext() : base("name=HighscoresContext")
        {

        }
    }
}
