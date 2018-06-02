using System;
using BowlingScore;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore.Test
{
    [TestClass]
    public class ScoreBoardCalculaterTester
    {
        [TestMethod]
        public void TestMethod1()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(1,4), new Frame(4,5), new Frame(6,4), new Frame(5,5),
                new Frame(10), new Frame(0,1), new Frame(7,3), new Frame(6,4),
                new Frame(10), new Frame(2,8,6) };

            testPlayer.frameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.calculateScoreBoards(new List<Player> { testPlayer });

            Assert.AreEqual(testPlayer.totalScore, 133);
        }
    }
}
