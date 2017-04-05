using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Grid
    {
        public enum Orientation { Horizontal, Vertical };
        public enum CaseState{Empty, Nothing, Hurted, Killed};

        private class ShipGridElement
        {
            public Ship Ship = null;
            public CaseState State = CaseState.Empty;
        }


        private ShipGridElement[][] ShipGrid;

        public class NullSizeException : Exception
        {
            public NullSizeException() : base() { }
            public NullSizeException(string message) : base(message) { }
            public NullSizeException(string message, System.Exception inner) : base(message, inner) { }

            protected NullSizeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }

        public class PositionOutOfRangeException : Exception
        {
            public PositionOutOfRangeException() : base() { }
            public PositionOutOfRangeException(string message) : base(message) { }
            public PositionOutOfRangeException(string message, System.Exception inner) : base(message, inner) { }

            protected PositionOutOfRangeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }

        public class ShipCrossException : Exception
        {
            public ShipCrossException() : base() { }
            public ShipCrossException(string message) : base(message) { }
            public ShipCrossException(string message, System.Exception inner) : base(message, inner) { }

            protected ShipCrossException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        }

        public Grid(uint sizeX, uint sizeY)
        {
            if(sizeX == 0 || sizeY == 0)
            {
                throw new NullSizeException("Your Grid cannot have 0 size");
            }

            ShipGrid = new ShipGridElement[sizeX][];

            for (int x = 0; x < ShipGrid.Length; x++)
            {
                ShipGrid[x] = new ShipGridElement[sizeY];
            }
        }

        public void AddShip(Ship ship, Position pos, Grid.Orientation orientation)
        {
            if(orientation == Orientation.Horizontal)
            {
                if (this.IsOutOfBound(new Position(pos.x + ship.Size, pos.y)))
                {
                    throw new PositionOutOfRangeException("Your ship is out of the grid range");
                }

                for(int x = 0; x < ship.Size; x++)
                {
                    if (ShipGrid[pos.x + x][pos.y].Ship != null)
                    {
                        throw new ShipCrossException("Your ship is over an other ship");
                    }

                    ShipGrid[pos.x + x][pos.y].Ship = ship;
                    ShipGrid[pos.x + x][pos.y].State = CaseState.Nothing;
                }
            }
            else
            {
                if (this.IsOutOfBound(new Position(pos.x, pos.y + ship.Size)))
                {
                    throw new PositionOutOfRangeException("Your ship is out of the grid range");
                }

                for (int y = 0; y < ship.Size; y++)
                {
                    if (ShipGrid[pos.x][pos.y + y].Ship != null)
                    {
                        throw new ShipCrossException("Your ship is over an other ship");
                    }

                    ShipGrid[pos.x][pos.y + y].Ship = ship;
                    ShipGrid[pos.x][pos.y + y].State = CaseState.Nothing;
                }
            }
        }

        public CaseState ShootCase(Position pos)
        {
            if (this.IsOutOfBound(pos))
            {
                throw new PositionOutOfRangeException("You tried to access a grid element out of range");
            }

            if (ShipGrid[pos.x][pos.y].State == CaseState.Nothing)
            {
                ShipGrid[pos.x][pos.y].State = CaseState.Hurted;
            }

            return ShipGrid[pos.x][pos.y].State;
        }

        public CaseState GetCaseState(Position pos)
        {
            if (this.IsOutOfBound(pos))
            {
                throw new PositionOutOfRangeException("You tried to access a grid element out of range");
            }

            return ShipGrid[pos.x][pos.y].State;
        }

        public bool IsOver()
        {
            for (int x = 0; x < ShipGrid.Length; x++)
            {
                for (int y = 0; y < ShipGrid[x].Length; y++)
                {
                    if (ShipGrid[x][y].State == CaseState.Nothing || ShipGrid[x][y].State == CaseState.Hurted)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsOutOfBound(Position pos)
        {
            return pos.x >= ShipGrid.Length || pos.y >= ShipGrid[0].Length;
        }

        private ShipGridElement[] GetCaseByShip(Ship ship)
        {
            ShipGridElement[] elements = new ShipGridElement[ship.Size];
            uint elementsIndex = 0;

            for(int x = 0; x < ShipGrid.Length; x++)
            {
                for(int y = 0; y < ShipGrid[x].Length; y++)
                {
                    if(ShipGrid[x][y].Ship == ship)
                    {
                        elements[elementsIndex] = ShipGrid[x][y];
                        elementsIndex++;
                    }
                }
            }

            return elements;
        }
    }
}
