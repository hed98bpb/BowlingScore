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
            while (IllegalNumberOfPlayers(numberOfPlayer))
            {
                Console.WriteLine("That doesn't seem right. Please enter a number between 1 and 4 :-)");
                numberOfPlayer = Console.ReadLine();
            }
            
            this.numberOfPlayers = int.Parse(numberOfPlayer);
        }

        private bool IllegalNumberOfPlayers(string numberOfPlayer)
        {
            return !Regex.IsMatch(numberOfPlayer, @"^([1-4])$"); ;
        }

        public void RunGame()
        {
            int numberOfFrames = 0;
            List<Player> playerList = CreatePlayerList(numberOfPlayers);
                
            // Handles first 9 round
            while (numberOfFrames < 9)
            {
                HandleRound(ref numberOfFrames, playerList);
            }

            Console.WriteLine("\nLast round!!!");

            LastRound(playerList);

            ScoreBoardCalculater cal = new ScoreBoardCalculater();
            cal.CalculateScoreBoards(playerList);

            Player winner = CalculateWinner(playerList);

            // doesnt account for ties
            Console.WriteLine("The winner is " + winner.playerName + " with " + winner.TotalScore + "points");

        }

        public Player CalculateWinner(List<Player> playerList)
        {
            return playerList.OrderBy(player => player.TotalScore).ToList().First();
        }

        public List<Player> CreatePlayerList(int numberOfPlayers)
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

                    if (player.Roll2 == 10)
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

                player.FrameHistory.Add(new Frame(player.Roll1, player.Roll2, player.Roll3));

                Console.WriteLine("That was it for " + player.playerName);

            }
        }

        public bool ThirdRollStrike(Player player)
        {
            if (player.Roll3 == 10)
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
                player.FrameHistory.Add(new Frame(player.Roll1));
            }
            else
            {
                AssignLegalSecondOrThirdRoll(player, 2);

                if (IsSpareAndPrints(player))
                {
                    player.SpareBonus = true;

                }

                player.FrameHistory.Add(new Frame(player.Roll1, player.Roll2));

            }

            player.Roll1 = 0;
            player.Roll2 = 0;
        }

        public bool IsSpareAndPrints(Player player)
        {
            if (player.Roll1 + player.Roll2 == 10)
            {
                Console.WriteLine("Spare!");
                return true;
            }
            return false;
        }

        public bool IsStrikeAndPrints(Player player)
        {
            if (player.Roll1 == 10)
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

            player.SetRoll(rollCounter, int.Parse(input));
        }

        public void AssignLegalSecondOrThirdRoll(Player player, int rollCounter)
        {
            AssignsIntegerInputToPlayersRoll(player, rollCounter);

            while (player.GetRoll(rollCounter - 1) + player.GetRoll(rollCounter) > 10)
            {
                Console.WriteLine("Second roll is too big, did you count right?");
                AssignsIntegerInputToPlayersRoll(player, rollCounter);
            }

        }
    }

    public class ScoreBoardCalculater
    {
        public void CalculateScoreBoards(List<Player> playerList)
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
                frame = player.FrameHistory[frameNumber];
                if (frame.IsStrikeFrame)
                {
                    CalculateStrikeFrame(player.FrameHistory, frame, frameNumber);
                }
                else if (frame.IsSpareFrame) 
                {
                    CalculateSpareFrame(player.FrameHistory, frame, frameNumber);
                }
                else if (frame.IsLastFrame)
                {
                    CalculateLastFrame(frame, frameNumber);
                }
                else CalculateRegularFrame(frame, frameNumber);

                player.TotalScore += frame.FrameScore;
            }
        }

        private void CalculateLastFrame(Frame frame, int frameNumber)
        {
            frame.FrameScore = frame.Roll1 + frame.Roll2 + frame.Roll3;
        }

        private void CalculateRegularFrame(Frame frame, int frameNumber)
        {
            frame.FrameScore = frame.Roll1 + frame.Roll2;
        }

        private void CalculateSpareFrame(List<Frame> frameHistory, Frame frame, int frameNumber)
        {
            frame.FrameScore = frame.Roll1 + frame.Roll2 + frameHistory[frameNumber + 1].Roll1;
        }

        public void CalculateStrikeFrame(List<Frame> frameHistory, Frame frame, int frameNumber)
        {
            Frame nextFrame = frameHistory[frameNumber + 1];

            if (nextFrame.IsStrikeFrame)
            {
                frame.FrameScore = frame.Roll1 + nextFrame.Roll1 + frameHistory[frameNumber + 2].Roll1;
            }
            else
            {
                frame.FrameScore = frame.Roll1 + nextFrame.Roll1 + nextFrame.Roll2;
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

        public int TotalScore { get; set; }
        public int NumberOfRolls { get; set; }
        public int OldRoll1 { get; set; }
        public int Roll1;
        public int Roll2;
        public int Roll3;
        public int AwardBonusForStrikeLeft { get; set; }
        public int AwardBonusForDoubleStrikeLeft { get; set; }

        public bool SpareBonus { get; set; } = false;
        public bool StrikeBonus { get; set; } = false;
        public bool DoubleStrikeBonus { get; set; } = false;

        public List<Frame> FrameHistory { get; set; } = new List<Frame>();

        public Player(string playername)
        {
            this.playerName = playername;
        }

        public void SetRoll(int rollCounter, int rollScore)
        {
            if (rollCounter == 1) this.Roll1 = rollScore;
            if (rollCounter == 2) this.Roll2 = rollScore;
            if (rollCounter == 3) this.Roll3 = rollScore;
        }

        public int GetRoll(int roll)
        {
            if (roll == 1) return Roll1;
            if (roll == 2) return Roll2;
            if (roll == 3) return Roll3;
            return 0;
        }

    }

    public class Frame
    {
        public bool IsStrikeFrame { get; } = false;
        public bool IsSpareFrame { get; } = false;
        public bool IsLastFrame { get; } = false;

        public int Roll1 { get; }
        public int Roll2 { get; }
        public int Roll3 { get; }

        public int FrameScore { get; set; } = 0;

        public Frame(int roll1)
        {
            this.Roll1 = roll1;
            IsStrikeFrame = true;
        }

        public Frame(int roll1, int roll2)
        {
            this.Roll1 = roll1;
            this.Roll2 = roll2;

            if (roll1 + roll2 == 10) IsSpareFrame = true;
            else IsSpareFrame = false;
        }

        public Frame(int roll1, int roll2, int roll3)
        {
            this.Roll1 = roll1;
            this.Roll2 = roll2;
            this.Roll3 = roll3;
            IsLastFrame = true;
        }


    }
}