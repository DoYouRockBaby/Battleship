using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Ship
    {
        public String Name { get; private set; }
        public uint Size { get; private set; }

        public Ship(String name, uint size)
        {
            Name = name;
            Size = size;
        }
    }
}
