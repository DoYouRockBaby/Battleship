using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship;

namespace GridTest
{
    [TestClass]
    public class GridTest
    {
        private Grid TestGrid;

        [TestInitialize]
        public void SetUp()
        {
            TestGrid = new Grid(10, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.NullSizeException))]
        public void NewGrid0SizeExceptionX()
        {
            new Grid(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.NullSizeException))]
        public void NewGrid0SizeExceptionY()
        {
            new Grid(1, 0);
        }

        [TestMethod]
        public void AddShipTest()
        {
            Ship ship1 = new Ship("Maximator", 4);
            Ship ship2 = new Ship("Navigator", 2);

            TestGrid.AddShip(ship1, new Position(4, 2), Grid.Orientation.Horizontal);
            TestGrid.AddShip(ship2, new Position(5, 4), Grid.Orientation.Vertical);

            Assert.AreEqual(Grid.CaseState.Empty, TestGrid.GetCaseState(new Position(3, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(4, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(6, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(7, 2)));
            Assert.AreEqual(Grid.CaseState.Empty, TestGrid.GetCaseState(new Position(8, 2)));
            Assert.AreEqual(Grid.CaseState.Empty, TestGrid.GetCaseState(new Position(5, 3)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 4)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 5)));
            Assert.AreEqual(Grid.CaseState.Empty, TestGrid.GetCaseState(new Position(5, 6)));
        }

        [TestMethod]
        public void ShipShootTest()
        {
            Ship ship1 = new Ship("Navigator", 2);

            //The ship is ok
            TestGrid.AddShip(ship1, new Position(4, 2), Grid.Orientation.Horizontal);
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(4, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 2)));

            //The ship is missed, still ok
            Grid.CaseState state1 = TestGrid.ShootCase(new Position(3, 2));
            Assert.AreEqual(Grid.CaseState.Empty, state1);
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(4, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 2)));

            //The ship is hurted
            Grid.CaseState state2 = TestGrid.ShootCase(new Position(4, 2));
            Assert.AreEqual(Grid.CaseState.Hurted, state2);
            Assert.AreEqual(Grid.CaseState.Hurted, TestGrid.GetCaseState(new Position(4, 2)));
            Assert.AreEqual(Grid.CaseState.Nothing, TestGrid.GetCaseState(new Position(5, 2)));

            //The ship is killed
            Grid.CaseState state3 = TestGrid.ShootCase(new Position(5, 2));
            Assert.AreEqual(Grid.CaseState.Killed, state3);
            Assert.AreEqual(Grid.CaseState.Killed, TestGrid.GetCaseState(new Position(4, 2)));
            Assert.AreEqual(Grid.CaseState.Killed, TestGrid.GetCaseState(new Position(5, 2)));
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.ShipCrossException))]
        public void AddShipCrossExceptionTest()
        {
            Ship addedShip1 = new Ship("Maximator", 4);
            Ship addedShip2 = new Ship("Navigator", 2);

            TestGrid.AddShip(addedShip1, new Position(4, 2), Grid.Orientation.Horizontal);
            TestGrid.AddShip(addedShip2, new Position(5, 1), Grid.Orientation.Vertical);
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.PositionOutOfRangeException))]
        public void AddShipPositionOutOfBoundTest()
        {
            Ship addedShip = new Ship("Navigator", 2);
            TestGrid.AddShip(addedShip, new Position(5, 10), Grid.Orientation.Horizontal);
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.PositionOutOfRangeException))]
        public void AddShipSizeOutOfBoundTest()
        {
            Ship addedShip = new Ship("Navigator", 2);
            TestGrid.AddShip(addedShip, new Position(5, 9), Grid.Orientation.Vertical);
        }

        [TestMethod]
        [ExpectedException(typeof(Grid.PositionOutOfRangeException))]
        public void GetCaseStateOutOfBoundTest()
        {
            TestGrid.GetCaseState(new Position(5, 10));
        }

        [TestMethod]
        public void IsOverTest()
        {
            Ship addedShip1 = new Ship("Maximator", 4);
            Ship addedShip2 = new Ship("Navigator", 2);

            TestGrid.AddShip(addedShip1, new Position(4, 2), Grid.Orientation.Horizontal);
            TestGrid.AddShip(addedShip2, new Position(5, 3), Grid.Orientation.Vertical);

            Assert.AreEqual(false, TestGrid.IsOver());

            //kill the first ship, the game is not over
            TestGrid.ShootCase(new Position(4, 2));
            TestGrid.ShootCase(new Position(5, 2));
            TestGrid.ShootCase(new Position(6, 2));
            TestGrid.ShootCase(new Position(7, 2));
            Assert.AreEqual(false, TestGrid.IsOver());

            //hurt the second ship, the game is not over
            TestGrid.ShootCase(new Position(5, 3));
            Assert.AreEqual(false, TestGrid.IsOver());

            //kill the second ship, the game is over
            TestGrid.ShootCase(new Position(5, 4));
            Assert.AreEqual(true, TestGrid.IsOver());
        }
    }
}
