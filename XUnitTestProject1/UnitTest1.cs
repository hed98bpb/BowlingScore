using System.Collections.Generic;
using Xunit;

namespace BowlingScore.Test
{
    
    public class ScoreBoardCalculaterTester
    {

        [Fact]
        public void ExamplePdfScore133()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(1,4), new Frame(4,5), new Frame(6,4),
                new Frame(5,5), new Frame(10), new Frame(0,1),
                new Frame(7,3), new Frame(6,4), new Frame(10),
                new Frame(2,8,6) };

            testPlayer.FrameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.CalculateScoreBoards(new List<Player> { testPlayer });

            Assert.Equal(testPlayer.TotalScore, 133);
        }

        [Fact]
        public void Perfect300Game()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(10), new Frame(10), new Frame(10),
                new Frame(10), new Frame(10), new Frame(10),
                new Frame(10), new Frame(10), new Frame(10),
                new Frame(10,10,10) };

            testPlayer.FrameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.CalculateScoreBoards(new List<Player> { testPlayer });

            Assert.Equal(testPlayer.TotalScore, 300);
        }

        [Fact]
        public void Bad0Game()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(0,0), new Frame(0,0), new Frame(0,0),
                new Frame(0,0), new Frame(0,0), new Frame(0,0),
                new Frame(0,0), new Frame(0,0), new Frame(0,0),
                new Frame(0,0)};

            testPlayer.FrameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.CalculateScoreBoards(new List<Player> { testPlayer });

            Assert.Equal(testPlayer.TotalScore, 0);
        }

        [Fact]
        public void A107Game()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(4,6), new Frame(10), new Frame(6,1),
                new Frame(2,6), new Frame(10), new Frame(3,0),
                new Frame(7,0), new Frame(3,5), new Frame(4,6),
                new Frame(5,4)};

            testPlayer.FrameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.CalculateScoreBoards(new List<Player> { testPlayer });

            Assert.Equal(testPlayer.TotalScore, 107);
        }

        [Fact]
        public void A129Game()
        {
            Player testPlayer = new Player("Player 1");

            List<Frame> testFrameHistory = new List<Frame> {
                new Frame(6,1), new Frame(4,0), new Frame(7,0),
                new Frame(8,1), new Frame(10), new Frame(10),
                new Frame(9,1), new Frame(9,1), new Frame(8,0),
                new Frame(8,0)};

            testPlayer.FrameHistory = testFrameHistory;

            ScoreBoardCalculater sbc = new ScoreBoardCalculater();
            sbc.CalculateScoreBoards(new List<Player> { testPlayer });

            Assert.Equal(testPlayer.TotalScore, 129);
        }
    }
}
