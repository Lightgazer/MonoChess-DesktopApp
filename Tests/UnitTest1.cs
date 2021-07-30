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
    }
}