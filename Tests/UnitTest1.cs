using MonoChess_DesktopApp.Draughts;
using MonoChess_DesktopApp.Draughts.Enums;
using MonoChess_DesktopApp.Draughts.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var expected = new List<int> { 30, 31, 32, 33, 34 };
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

        [Test]
        public void ExecuteCommand()
        {
            var model = new DraughtsModel();
            var commands = model.GetPossibleCommands(30);
            var leftCommand = commands[0];
            model.Execute(leftCommand);
            var active = model.GetActivePieces();
            var expected = new List<int> { 15, 16, 17, 18, 19 };
            CollectionAssert.AreEqual(active, expected);
        }

        [Test]
        public void CapturePiece()
        {
            var model = new DraughtsModel();
            var commands = model.GetPossibleCommands(30);
            model.Execute(commands[1]);
            var commands2 = model.GetPossibleCommands(15);
            model.Execute(commands2[0]);

            var active = model.GetActivePieces();
            var expected = new List<int> { 26 };
            CollectionAssert.AreEqual(active, expected);

            var commands3 = model.GetPossibleCommands(26);
            model.Execute(commands3[0]);

            var positions = model.GetPiecePositions();
            Assert.AreEqual(positions[15], PieceType.WhitePvt);
        }

        [Test]
        public void CapturePiece2()
        {
            var model = new DraughtsModel();
            var commands = model.GetPossibleCommands(30);
            model.Execute(commands[0]);
            var commands2 = model.GetPossibleCommands(16);
            model.Execute(commands2[0]);
            var active = model.GetActivePieces();
            var expected = new List<int> { 25 };
            CollectionAssert.AreEqual(active, expected);
            var commands3 = model.GetPossibleCommands(25);
            model.Execute(commands3[0]);
            var positions = model.GetPiecePositions();
            Assert.AreEqual(positions[16], PieceType.WhitePvt);
        }

        public void CrowningPiece()
        {
            var pieces = new PieceType[DraughtsConstants.NumberOfPositions];
            Array.Fill(pieces, PieceType.BlackPvt, 0, 20);
            Array.Fill(pieces, PieceType.WhitePvt, 30, 20);
            pieces[1] = PieceType.None;
            pieces[10] = PieceType.WhitePvt;

            var model = new DraughtsModel(new Turn(pieces, Side.White));
            var active = model.GetActivePieces();
            var commands = model.GetPossibleCommands(active[0]);
            model.Execute(commands[0]);
            var positions = model.GetPiecePositions();
            Assert.AreEqual(positions[1], PieceType.WhiteKing);
        }
    }
}