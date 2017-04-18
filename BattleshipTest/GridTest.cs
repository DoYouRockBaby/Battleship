using System;
using NUnit.Framework;
using Battleship;

namespace GridTest
{
    [TestFixture]
    public class GridTest
    {
        private Grid TestGrid;

        [SetUpFixture]
        public void SetUp()
        {
            TestGrid = new Grid(10, 10);
        }

        [Test]
        public void NewGrid0SizeExceptionX()
        {
            Assert.Throws<Grid.NullSizeException>(delegate
            {
                new Grid(0, 1);
            });
        }

        [Test]
        public void NewGrid0SizeExceptionY()
        {
            Assert.Throws<Grid.NullSizeException>(delegate
            {
                new Grid(1, 0);
            });
        }

        [Test]
        public void AddShipTest()
        {
            Ship ship1 = new Ship("Maximator", 4);
            Ship ship2 = new Ship("Navigator", 2);

            TestGrid.AddShip(ship1, new Position(4, 2), Grid.Orientation.Horizontal);
            TestGrid.AddShip(ship2, new Position(5, 4), Grid.Orientation.Vertical);

            Assert.That(Grid.CaseState.Empty, Is.EqualTo(TestGrid.GetCaseState(new Position(3, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(4, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(6, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(7, 2))));
            Assert.That(Grid.CaseState.Empty, Is.EqualTo(TestGrid.GetCaseState(new Position(8, 2))));
            Assert.That(Grid.CaseState.Empty, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 3))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 4))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 5))));
            Assert.That(Grid.CaseState.Empty, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 6))));

            Assert.That(true, Is.EqualTo(false));
        }

        [Test]
        public void ShipShootTest()
        {
            Ship ship1 = new Ship("Navigator", 2);

            //The ship is ok
            TestGrid.AddShip(ship1, new Position(4, 2), Grid.Orientation.Horizontal);
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(4, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 2))));

            //The ship is missed, still ok
            Grid.CaseState state1 = TestGrid.ShootCase(new Position(3, 2));
            Assert.That(Grid.CaseState.Empty, Is.EqualTo(state1));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(4, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 2))));

            //The ship is hurted
            Grid.CaseState state2 = TestGrid.ShootCase(new Position(4, 2));
            Assert.That(Grid.CaseState.Hurted, Is.EqualTo(state2));
            Assert.That(Grid.CaseState.Hurted, Is.EqualTo(TestGrid.GetCaseState(new Position(4, 2))));
            Assert.That(Grid.CaseState.Nothing, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 2))));

            //The ship is killed
            Grid.CaseState state3 = TestGrid.ShootCase(new Position(5, 2));
            Assert.That(Grid.CaseState.Killed, Is.EqualTo(state3));
            Assert.That(Grid.CaseState.Killed, Is.EqualTo(TestGrid.GetCaseState(new Position(4, 2))));
            Assert.That(Grid.CaseState.Killed, Is.EqualTo(TestGrid.GetCaseState(new Position(5, 2))));
        }

        [Test]
        public void AddShipCrossExceptionTest()
        {
            Ship addedShip1 = new Ship("Maximator", 4);
            Ship addedShip2 = new Ship("Navigator", 2);

            TestGrid.AddShip(addedShip1, new Position(4, 2), Grid.Orientation.Horizontal);

            Assert.Throws<Grid.ShipCrossException>(delegate
            {
                TestGrid.AddShip(addedShip2, new Position(5, 1), Grid.Orientation.Vertical);
            });
        }

        [Test]
        public void AddShipPositionOutOfBoundTest()
        {
            Ship addedShip = new Ship("Navigator", 2);
            Assert.Throws<Grid.PositionOutOfRangeException>(delegate
            {
                TestGrid.AddShip(addedShip, new Position(5, 10), Grid.Orientation.Horizontal);
            });
        }

        [Test]
        public void AddShipSizeOutOfBoundTest()
        {
            Ship addedShip = new Ship("Navigator", 2);
            Assert.Throws<Grid.PositionOutOfRangeException>(delegate
            {
                TestGrid.AddShip(addedShip, new Position(5, 9), Grid.Orientation.Vertical);
            });
        }

        [Test]
        public void GetCaseStateOutOfBoundTest()
        {
            Assert.Throws<Grid.PositionOutOfRangeException>(delegate
            {
                TestGrid.GetCaseState(new Position(5, 10));
            });
        }

        [Test]
        public void IsOverTest()
        {
            Ship addedShip1 = new Ship("Maximator", 4);
            Ship addedShip2 = new Ship("Navigator", 2);

            TestGrid.AddShip(addedShip1, new Position(4, 2), Grid.Orientation.Horizontal);
            TestGrid.AddShip(addedShip2, new Position(5, 3), Grid.Orientation.Vertical);

            Assert.That(false, Is.EqualTo(TestGrid.IsOver()));

            //kill the first ship, the game is not over
            TestGrid.ShootCase(new Position(4, 2));
            TestGrid.ShootCase(new Position(5, 2));
            TestGrid.ShootCase(new Position(6, 2));
            TestGrid.ShootCase(new Position(7, 2));
            Assert.That(false, Is.EqualTo(TestGrid.IsOver()));

            //hurt the second ship, the game is not over
            TestGrid.ShootCase(new Position(5, 3));
            Assert.That(false, Is.EqualTo(TestGrid.IsOver()));

            //kill the second ship, the game is over
            TestGrid.ShootCase(new Position(5, 4));
            Assert.That(true, Is.EqualTo(TestGrid.IsOver()));
        }
    }
}
