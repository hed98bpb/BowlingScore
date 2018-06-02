The program is comprised of 5 classes

All these 5 classes are in the same filee to give a better overview for the reviewer. One 
could've easily distributed them into each their own files, but for simplicity i didn't.

- BowlingScore
	The BowlingScore class is basicly the main method. 
	It starts the BowlingGame class which is one of the two heavier classes, and upon
	end game, asks if the player want to play more, or exit.

- BowlingGame
	The BowlingGame class basicly takes the player(s) through each round of the game
	where they can type there input for each roll. It's also in charge of updating the 
	Players frameHistory, and making sure only valid inputs can be are saved, e.g. if
	the first roll yields a 7, then one can't roll 4 in the next roll, since there are 
	only 10 pins. So overall the BowlingGame class will provide a valid framehistory
	for each player, which the ScoreBoardCalculator will process.

	Should one want to only use the ScoreBoardCalculator, one would need to extract
	the logic of this class to check if the input to the ScoreBoardCalculator was well
	formed. One could do this by basicly simulating the game before parsing it as input.

- ScoreBoardCalculator
	The ScoreBoadCalculator is in charge of calculating the players score at the end 
	of the game. It's basicly the meat of algorithm described in the given pdf file,
	and therefore encapsulates of the scoring rules in bowling.

- Player
	The Player class is basicly a container for the information gathered through the
	game, which the ScoreBoadCalculator utilizes to calculate the final score.

- Frame
	The Frame class is a container for each of the frames played throughout the game.
	It basicly just keeps score of the different rolls, and whether it was a strike,
	spare, regular or last frame.

Besides this there is a UnitTest1 file which has 5 games of different nature.
