using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Position
    {
        public uint x;
        public uint y;

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
