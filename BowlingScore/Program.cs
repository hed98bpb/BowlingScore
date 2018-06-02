using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    class BowlingScore
    {
        static void Main(string[] args)
        {
            string input = "";

            while (true)
            {

                Console.WriteLine("Hi! Please enter your bowling score");

                RunGame();

                Console.WriteLine("Press 'x' to quit, or press 'p' to play again");

                input = Console.ReadLine();

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

        static void RunGame()
        {
            int numberOfFrames = 0;

            Player player1 = new Player("Player 1");
            Player player2 = new Player("Player 2");

            List<Player> playerList = new List<Player> { player1, player2 };

            // Handles first 9 round
            while (numberOfFrames <= 8)
            {
                HandleRound(ref numberOfFrames, playerList);
            }

            Console.WriteLine("Last round!!!");

            LastRound(playerList);

            if (player1.playerScore > player2.playerScore) Console.WriteLine("The winner is " + player1.playerName + " with " + player1.playerScore + "points");
            else if (player1.playerScore < player2.playerScore) Console.WriteLine("The winner is " + player2.playerName + " with " + player2.playerScore + "points");
            else Console.WriteLine("Wow it's a tie with " + player1.playerScore + " to both players!");

        }

        static void HandleRound(ref int numberOfFrames, List<Player> playerList)
        {
            numberOfFrames++;

            Console.WriteLine("\nRound: " + numberOfFrames + "!");
            
            foreach (Player player in playerList)
            {
                OneFrameForOnePlayer(player);
            }
        }

        static void LastRound(List<Player> playerList)
        {

            foreach (Player player in playerList)
            {

                Console.WriteLine("\n" + player.playerName + " to bowl");

                AssignsIntegerInputToPlayersRoll(player, 1);
                UpdatePlayerScore(player, 1);

                HandleBonuses(player);

                if (IsStrikeFrame(player))
                {
                    AssignsIntegerInputToPlayersRoll(player, 2);
                    UpdatePlayerScore(player, 2);

                    HandleBonuses(player);

                    Console.WriteLine("\nBonus roll!");

                    AssignsIntegerInputToPlayersRoll(player, 1);
                    UpdatePlayerScore(player, 1);
                }
                else
                {
                    AssignsIntegerInputToPlayersRoll(player, 2);
                    UpdatePlayerScore(player, 2);

                    HandleBonuses(player);

                    if (IsSpareFrame(player))
                    {
                        Console.WriteLine(player.playerName + "'s score: " + player.playerScore);
                        Console.WriteLine("\nBonus roll!");
                        
                        AssignsIntegerInputToPlayersRoll(player, 1);
                        UpdatePlayerScore(player, 1);
                    }
                }

                Console.WriteLine(player.playerName + "'s final score: " + player.playerScore);

            }
        }

        static void OneFrameForOnePlayer(Player player)
        {
            Console.WriteLine("\n" + player.playerName + " to bowl");

            AssignsIntegerInputToPlayersRoll(player, 1);

            HandleBonuses(player);
            
            if (IsStrikeFrame(player))
            {
                HandleStrikeFrame(player);

                Console.WriteLine(player.playerName + "'s total score: " + player.playerScore);
            }
            else
            {
                UpdatePlayerScore(player, 1);

                AssignsIntegerInputToPlayersRoll(player, 2);
                AssignLegalSecondThrow(player);

                if (player.strikeBonus) HandleStrikeBonus(player, 2);

                if (IsSpareFrame(player))
                {
                    player.spareBonus = true;
                    Console.WriteLine("Spare!");
                }

                UpdatePlayerScore(player, 2);

                Console.WriteLine(player.playerName + "'s score: " + player.playerScore);
            }

            player.roll1 = 0;
            player.roll2 = 0;
        }

        static void HandleStrikeFrame(Player player)
        {

            if (IsDoubleStrikeFrame(player))
            {
                player.rolledADoubleStrike();
                UpdatePlayerScore(player, 1);
            }
            else if (player.strikeBonus)
            {
                player.rolledAStrike();
                UpdatePlayerScore(player, 1);
            }
            else
            {
                player.rolledAStrike();
                UpdatePlayerScore(player, 1);
            }
        }

        static void HandleBonuses(Player player)
        {
            if (player.doubleStrikeBonus) HandleDoubleStrikeBonus(player, 1);
            else if (player.strikeBonus) HandleStrikeBonus(player, 1);
            else if (player.spareBonus) HandleSpareBonus(player);
            
        }

        static void HandleDoubleStrikeBonus(Player player, int rollCounter)
        {
            if (player.awardBonusForDoubleStrikeLeft == 1)
            {
                player.playerScore += 2 * player.getRoll(rollCounter);
                player.awardBonusForDoubleStrikeLeft--;
                player.awardBonusForStrikeLeft--;
                player.doubleStrikeBonus = false;
            }
        }

        static void HandleSpareBonus(Player player)
        {
            player.playerScore += player.getRoll(1);
            player.spareBonus = false;
        }

        static void HandleStrikeBonus(Player player, int rollCounter)
        {
            if (player.awardBonusForStrikeLeft == 2)
            {
                player.playerScore += player.getRoll(rollCounter);
                player.awardBonusForStrikeLeft--;
            } else if (player.awardBonusForStrikeLeft == 1)
            {
                player.playerScore += player.getRoll(rollCounter);
                player.awardBonusForStrikeLeft--;
                player.strikeBonus = false;
            }

        }

        static void UpdatePlayerScore(Player player, int rollCounter)
        {
            if (rollCounter == 1) player.oldRoll1 = player.roll1;

            player.playerScore += player.getRoll(rollCounter);
        }

        static bool IsSpareFrame(Player player)
        {
            return player.roll1 + player.roll2 == 10;
        }

        static bool IsStrikeFrame(Player player)
        {
            return player.roll1 == 10;
        }

        static bool IsDoubleStrikeFrame(Player player)
        {
            return player.oldRoll1 == 10 && player.roll1 == 10;
        }

        static bool CheckForLegalIntegerInput(string input)
        {
            return Regex.IsMatch(input, @"^([0-9]|10)$"); ;
        }

        static void AssignsIntegerInputToPlayersRoll(Player player, int rollCounter)
        {
            string input = Console.ReadLine();

            while (!CheckForLegalIntegerInput(input))
            {
                Console.WriteLine("Try a legal input :)");
                input = Console.ReadLine();
            }

            player.setRoll(rollCounter, int.Parse(input));
        }

        static void AssignLegalSecondThrow(Player player)
        {

            while(player.roll1 + player.roll2 > 10)
            {
                Console.WriteLine("Second roll is too big, did you count right?");
                AssignsIntegerInputToPlayersRoll(player, 2);
            }

        }

    }

    class Player
    {
        public string playerName;

        public int playerScore { get; set; }
        public int numberOfRolls { get; set; }
        public int oldRoll1 { get; set; }
        public int roll1;
        public int roll2;
        public int awardBonusForStrikeLeft { get; set; }
        public int awardBonusForDoubleStrikeLeft { get; set; }

        public bool spareBonus { get; set; }
        public bool strikeBonus { get; set; }
        public bool doubleStrikeBonus { get; set; }

        public Player(string playername)
        {
            this.playerName = playername;
            this.playerScore = 0;
            this.oldRoll1 = 0;
            this.roll1 = 0;
            this.roll2 = 0;
            this.spareBonus = false;
            this.strikeBonus = false;
            this.doubleStrikeBonus = false;
            this.awardBonusForStrikeLeft = 0;
            this.awardBonusForDoubleStrikeLeft = 0;
        }

        public void setRoll(int rollCounter, int rollScore)
        {
            if (rollCounter == 1) this.roll1 = rollScore;
            if (rollCounter == 2) this.roll2 = rollScore;
        }

        public int getRoll(int roll)
        {
            if (roll == 1) return roll1;
            if (roll == 2) return roll2;
            return 0;
        }

        public void rolledAStrike()
        {
            Console.WriteLine("Strike!");
            strikeBonus = true;
            awardBonusForStrikeLeft = 2;
        }

        public void rolledADoubleStrike()
        {
            Console.WriteLine("Double Strike!!");
            strikeBonus = true;
            doubleStrikeBonus = true;
            awardBonusForStrikeLeft = 2;
            awardBonusForDoubleStrikeLeft = 1;
        }

    }
}