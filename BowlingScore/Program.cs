using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    public class BowlingScore
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Hi! How many will be playing?");

                BowlingGame game = new BowlingGame();
                game.RunGame();

                Console.WriteLine("Press 'x' to quit, or press 'p' to play again");

                string input = Console.ReadLine();

                while (!(input.Equals("x") || input.Equals("p")))
                {
                    Console.WriteLine("oops! That's not one of the options, either press 'p' or 'x'");
                    input = Console.ReadLine();
                }
                
                if (input == "x")
                {
                    break;
                }
                    

            }

        }

    }

    public class BowlingGame
    {
        int numberOfPlayers;

        public BowlingGame()
        {
            string numberOfPlayer = Console.ReadLine();
            while (illegalNumberOfPlayers(numberOfPlayer))
            {
                Console.WriteLine("That doesn't seem right. Please enter a number between 1 and 4 :-)");
                numberOfPlayer = Console.ReadLine();
            }
            
            this.numberOfPlayers = int.Parse(numberOfPlayer);
        }

        private bool illegalNumberOfPlayers(string numberOfPlayer)
        {
            return !Regex.IsMatch(numberOfPlayer, @"^([1-4])$"); ;
        }

        public void RunGame()
        {
            int numberOfFrames = 0;
            List<Player> playerList = createPlayerList(numberOfPlayers);
                
            // Handles first 9 round
            while (numberOfFrames < 9)
            {
                HandleRound(ref numberOfFrames, playerList);
            }

            Console.WriteLine("\nLast round!!!");

            LastRound(playerList);

            ScoreBoardCalculater cal = new ScoreBoardCalculater();
            cal.calculateScoreBoards(playerList);

            Player winner = calculateWinner(playerList);

            // doesnt account for ties
            Console.WriteLine("The winner is " + winner.playerName + " with " + winner.totalScore + "points");

        }

        public Player calculateWinner(List<Player> playerList)
        {
            return playerList.OrderBy(player => player.totalScore).ToList().First();
        }

        public List<Player> createPlayerList(int numberOfPlayers)
        {
            List<Player> playerList = new List<Player>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerList.Add(new Player("Player " + (i + 1)));
            }

            return playerList;
        }

        public void HandleRound(ref int numberOfFrames, List<Player> playerList)
        {
            numberOfFrames++;

            Console.WriteLine("\nRound: " + numberOfFrames + "!");

            foreach (Player player in playerList)
            {
                OneFrameForOnePlayer(player);
            }
        }

        public void LastRound(List<Player> playerList)
        {

            foreach (Player player in playerList)
            {
                Console.WriteLine("\n" + player.playerName + " to bowl");

                AssignsIntegerInputToPlayersRoll(player, 1);

                if (IsStrikeAndPrints(player))
                {
                    AssignsIntegerInputToPlayersRoll(player, 2);

                    if (player.roll2 == 10)
                    {
                        Console.WriteLine("Strike!");

                        AssignsIntegerInputToPlayersRoll(player, 3);

                        if (ThirdRollStrike(player))
                        {

                        }
                    }
                    else
                    {
                        AssignLegalSecondOrThirdRoll(player, 3);
                    }

                }
                else
                {
                    AssignLegalSecondOrThirdRoll(player, 2);

                    if (IsSpareAndPrints(player))
                    {
                        AssignsIntegerInputToPlayersRoll(player, 3);

                        if (ThirdRollStrike(player))
                        {

                        }

                    }

                }

                player.frameHistory.Add(new Frame(player.roll1, player.roll2, player.roll3));

                Console.WriteLine("That was it for " + player.playerName);

            }
        }

        public bool ThirdRollStrike(Player player)
        {
            if (player.roll3 == 10)
            {
                Console.WriteLine("Strike!");
                return true;
            }
            return false;
        }

        public void OneFrameForOnePlayer(Player player)
        {
            Console.WriteLine("\n" + player.playerName + " to bowl");

            AssignsIntegerInputToPlayersRoll(player, 1);

            if (IsStrikeAndPrints(player))
            {
                player.frameHistory.Add(new Frame(player.roll1));
            }
            else
            {
                AssignLegalSecondOrThirdRoll(player, 2);

                if (IsSpareAndPrints(player))
                {
                    player.spareBonus = true;

                }

                player.frameHistory.Add(new Frame(player.roll1, player.roll2));

            }

            player.roll1 = 0;
            player.roll2 = 0;
        }

        public bool IsSpareAndPrints(Player player)
        {
            if (player.roll1 + player.roll2 == 10)
            {
                Console.WriteLine("Spare!");
                return true;
            }
            return false;
        }

        public bool IsStrikeAndPrints(Player player)
        {
            if (player.roll1 == 10)
            {
                Console.WriteLine("Strike!");
                return true;
            }

            return false;
        }

        public bool CheckForLegalIntegerRoll(string input)
        {
            return Regex.IsMatch(input, @"^([0-9]|10)$"); ;
        }

        public void AssignsIntegerInputToPlayersRoll(Player player, int rollCounter)
        {
            string input = Console.ReadLine();

            while (!CheckForLegalIntegerRoll(input))
            {
                Console.WriteLine("Try a legal input :)");
                input = Console.ReadLine();
            }

            player.setRoll(rollCounter, int.Parse(input));
        }

        public void AssignLegalSecondOrThirdRoll(Player player, int rollCounter)
        {
            AssignsIntegerInputToPlayersRoll(player, rollCounter);

            while (player.getRoll(rollCounter - 1) + player.getRoll(rollCounter) > 10)
            {
                Console.WriteLine("Second roll is too big, did you count right?");
                AssignsIntegerInputToPlayersRoll(player, rollCounter);
            }

        }
    }

    public class ScoreBoardCalculater
    {
        public void calculateScoreBoards(List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                CalculatePlayersScoreBoard(player);
            }
        }
        
        public void CalculatePlayersScoreBoard(Player player)
        {
            Frame frame;
            for (int frameNumber = 0; frameNumber < 10; frameNumber++)
            {
                frame = player.frameHistory[frameNumber];
                if (frame.isStrikeFrame)
                {
                    calculateStrikeFrame(player.frameHistory, frame, frameNumber);
                }
                else if (frame.isSpareFrame) 
                {
                    calculateSpareFrame(player.frameHistory, frame, frameNumber);
                }
                else if (frame.isLastFrame)
                {
                    calculateLastFrame(frame, frameNumber);
                }
                else calculateRegularFrame(frame, frameNumber);

                player.totalScore += frame.frameScore;
            }
        }

        private void calculateLastFrame(Frame frame, int frameNumber)
        {
            frame.frameScore = frame.roll1 + frame.roll2 + frame.roll3;
        }

        private void calculateRegularFrame(Frame frame, int frameNumber)
        {
            frame.frameScore = frame.roll1 + frame.roll2;
        }

        private void calculateSpareFrame(List<Frame> frameHistory, Frame frame, int frameNumber)
        {
            frame.frameScore = frame.roll1 + frame.roll2 + frameHistory[frameNumber + 1].roll1;
        }

        public void calculateStrikeFrame(List<Frame> frameHistory, Frame frame, int frameNumber)
        {
            Frame nextFrame = frameHistory[frameNumber + 1];

            if (nextFrame.isStrikeFrame)
            {
                frame.frameScore = frame.roll1 + nextFrame.roll1 + frameHistory[frameNumber + 2].roll1;
            }
            else
            {
                frame.frameScore = frame.roll1 + nextFrame.roll1 + nextFrame.roll2;
            }
        }

        public bool IsStrike(Player player, int frame)
        {
            return false;
        }
    }

    public class Player
    {
        public string playerName;

        public int totalScore { get; set; }
        public int numberOfRolls { get; set; }
        public int oldRoll1 { get; set; }
        public int roll1;
        public int roll2;
        public int roll3;
        public int awardBonusForStrikeLeft { get; set; }
        public int awardBonusForDoubleStrikeLeft { get; set; }

        public bool spareBonus { get; set; } = false;
        public bool strikeBonus { get; set; } = false;
        public bool doubleStrikeBonus { get; set; } = false;

        public List<Frame> frameHistory { get; set; } = new List<Frame>();

        public Player(string playername)
        {
            this.playerName = playername;
        }

        public void setRoll(int rollCounter, int rollScore)
        {
            if (rollCounter == 1) this.roll1 = rollScore;
            if (rollCounter == 2) this.roll2 = rollScore;
            if (rollCounter == 3) this.roll3 = rollScore;
        }

        public int getRoll(int roll)
        {
            if (roll == 1) return roll1;
            if (roll == 2) return roll2;
            if (roll == 3) return roll3;
            return 0;
        }

    }

    public class Frame
    {
        public bool isStrikeFrame { get; } = false;
        public bool isSpareFrame { get; } = false;
        public bool isLastFrame { get; } = false;

        public int roll1 { get; }
        public int roll2 { get; }
        public int roll3 { get; }

        public int frameScore { get; set; } = 0;

        public Frame(int roll1)
        {
            this.roll1 = roll1;
            isStrikeFrame = true;
        }

        public Frame(int roll1, int roll2)
        {
            this.roll1 = roll1;
            this.roll2 = roll2;

            if (roll1 + roll2 == 10) isSpareFrame = true;
            else isSpareFrame = false;
        }

        public Frame(int roll1, int roll2, int roll3)
        {
            this.roll1 = roll1;
            this.roll2 = roll2;
            this.roll3 = roll3;
            isLastFrame = true;
        }


    }
}