using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Grid
    {
        private class ShipGridElement
        {
            public Ship Ship { get; set; }
            public Boolean Alive { get; set; }
        }

        public enum Orientation { Horizontal, Vertical };

        private ShipGridElement[][] ShipGrid;

        public class PositionOutOfRangeException : Exception
        {
        }

        public class ShipCrossException : Exception
        {
        }

        public enum CaseState{Empty, Nothing, Hurted, Killed};

        public Grid(uint sizeX, uint sizeY)
        {
        }

        public void AddShip(Ship ship, Position pos, Grid.Orientation orientation)
        {
        }

        public CaseState ShootCase(Position pos)
        {
            return CaseState.Nothing;
        }

        public CaseState GetCaseState(Position pos)
        {
            return CaseState.Nothing;
        }

        public bool IsOver()
        {
            return false;
        }
    }
}
