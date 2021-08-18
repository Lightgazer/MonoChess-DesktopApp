using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using MonoChess_DesktopApp.Draughts;

namespace Tests
{
    [TestFixture]
    public class DraughtsTests
    {
        [Test]
        public void InitialPositions()
        {
            var model = new DraughtsModel();
            var positions = model.GetPiecePositions();
            Assert.AreEqual(positions.Distinct().Count(), 3);
            Assert.AreEqual(positions.Count(piece => piece == PieceType.BlackPvt), 20);
            Assert.AreEqual(positions.Count(piece => piece == PieceType.WhitePvt), 20);
        }

        [Test]
        public void InitialActivePieces()
        {
            var model = new DraughtsModel();
            var active = model.GetActivePieces();
            var expected = new List<int> {30, 31, 32, 33, 34};
            CollectionAssert.AreEqual(active, expected);
        }

        [Test]
        public void PossibleActionsAtBeginning()
        {
            var model = new DraughtsModel();
            var commands = model.GetPossibleCommands(30);
            Assert.AreEqual(commands.Count, 2);
            Assert.AreEqual(commands[0].EndPosition, 25);
            Assert.AreEqual(commands[1].EndPosition, 26);
        }

        [Test]
        public void ImpossibleActionsAtBeginning()
        {
            var model = new DraughtsModel();
            var commands = model.GetPossibleCommands(25);
            Assert.AreEqual(commands.Count, 0);
        }
    }
}