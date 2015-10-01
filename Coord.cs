using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonWalker
{
    public class Coord
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coord()
        {
            this.X = 0;
            this.Y = 0;
        }
    }
}
