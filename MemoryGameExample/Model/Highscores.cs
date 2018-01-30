using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGameExample.Model
{
    public class Highscores
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public int Moves { get; set; }
    }
}
