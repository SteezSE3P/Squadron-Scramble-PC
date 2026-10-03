namespace SquadronScramble;

public class SquadronNames
{
	private GameWorld g;

	private string[] squadronColorNames = new string[4] { "WHITE SQUAD", "YELLOW SQUAD", "GREEN SQUAD", "BLUE SQUAD" };

	private string[] squadronNames = new string[8] { "'A' GROUP", "'B' GROUP", "'C' GROUP", "'D' GROUP", "'E' GROUP", "'F' GROUP", "'G' GROUP", "'H' GROUP" };

	private string[,] originalNames = new string[8, 36]
	{
		{
			"Ace", "Hector", "Blinky", "Goldie", "Bertie", "Viper", "Pancake", "Nozzle", "Icer", "Flyboy",
			"Weedy", "Chuckles", "Rookie", "Dreamer", "Guzzler", "Meddles", "Clocker", "Turbo", "Zero", "Budget",
			"Dribbler", "Fisher", "Buggy", "Slick", "Arrow", "Jaunty", "Sicknote", "Flash", "Beefcake", "Hippy",
			"Falcon", "Kettle", "Disco", "Klunk", "Boston", "Lucky"
		},
		{
			"Loopy", "Dippy", "Banky", "Mayday", "Joker", "Kicker", "Moggy", "Topsy", "Skipper", "Dimples",
			"Lounger", "Griff", "Charmer", "Chatty", "Bandit", "Doily", "Arty", "Winger", "Fine", "Freddy",
			"Muscles", "Howard", "Brick", "Goofer", "Champ", "Smidget", "Poltroon", "Noggin", "Boss", "Undies",
			"Loser", "Malarkey", "Sloosh", "Spike", "Jumper", "Bandage"
		},
		{
			"Rudder", "Splutter", "Gonner", "Scooter", "Igloo", "Moose", "Ashy", "Binbag", "Bander", "Hatchet",
			"Handle", "Oldie", "Gastro", "Nibbler", "Agro", "Bendy", "Chief", "Klutz", "Oddball", "Thruster",
			"Dollop", "Blimp", "Ghost", "Armpit", "Doorstop", "Bandana", "Piccolo", "Harpoon", "Sparky", "Buck",
			"Claxton", "Cockles", "Doozy", "Mitch", "Bouffant", "Cushty"
		},
		{
			"Chocks", "Pitcher", "Snapper", "Herbert", "Dude", "Firkin", "Box", "Junior", "Deadshot", "Bowyang",
			"Gears", "Bunty", "Adlib", "Mugwump", "Nitwit", "Bugle", "Roller", "Spork", "Benson", "Bagsy",
			"Giggles", "Teapot", "Whooper", "Staller", "Crowbar", "Frazzle", "Flat", "Sub", "Boggie", "Amigo",
			"Firkin", "Piffler", "Alfie", "Red", "Goose", "Helmet"
		},
		{
			"Radar", "Gauger", "King", "Noodle", "Rascal", "Mac", "Adonis", "Dinky", "Cheesy", "Juggles",
			"Scally", "Hoopla", "Jenkins", "Tiggy", "Boz", "Potty", "Porkie", "Skippy", "Seesaw", "Giddy",
			"Bell", "Smudge", "Quaggy", "Tash", "Boots", "Pickle", "Hazy", "Bernard", "Nimble", "Titch",
			"Charlie", "Muffin", "Dobber", "Yabby", "Rex", "Green"
		},
		{
			"Proppy", "Hotel", "Cobra", "Drooler", "Tango", "Sir", "Bomber", "Clipper", "Sandy", "Marbles",
			"Weasel", "Leech", "Jester", "Cracker", "Zed", "Curly", "Fuzzy", "Rambler", "Breezy", "Jeeves",
			"Noisy", "Sham", "Archie", "Nutmeg", "Lofty", "Mike", "Omega", "Mandarin", "Delta", "Echo",
			"Blip", "Egg", "Plonker", "Juicer", "Cuppa", "Bravo"
		},
		{
			"Boom", "Zoom", "Hunter", "Landfill", "Plunger", "Storm", "Yodeler", "Pointy", "Hotshot", "Tempo",
			"Quackers", "Normal", "Idle", "Juicer", "Ulterior", "Drafty", "Eerie", "Mug", "Monster", "Clinger",
			"Grower", "Folder", "Vest", "Xenon", "Aimless", "Knuckle", "Blister", "Waddler", "Octagon", "Razor",
			"Shallow", "Muncher", "Towers", "Parched", "Diddly", "Mojo"
		},
		{
			"Mayday", "Ambler", "Barrel", "Jimjams", "Nosedive", "Verbal", "Tailspin", "Handsome", "Ergo", "Kite",
			"Crumb", "Moper", "Pedant", "Donut", "Frantic", "Plummet", "Fallow", "Snoozer", "Techno", "Spry",
			"Gullet", "Mister", "Quaver", "Howler", "Bother", "Prism", "Sarky", "Wailer", "Rewind", "Chops",
			"Fuzzy", "Detour", "Prof", "Laiko", "Ominous", "Headache"
		}
	};

	private string[,] names;

	public SquadronNames(GameWorld gw)
	{
		g = gw;
		names = new string[8, 36];
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 36; j++)
			{
				names[i, j] = originalNames[i, j].ToString();
			}
		}
	}

	public string GetName(int s, int i)
	{
		return names[s, i];
	}

	public string ResetOriginalName(int s, int i)
	{
		names[s, i] = originalNames[s, i].ToString();
		return originalNames[s, i].ToString();
	}

	public void SetName(int s, int i, string st)
	{
		names[s, i] = st;
	}

	public string GetSquadronName(int s)
	{
		return squadronNames[s];
	}

	public string[,] GetNames()
	{
		return names;
	}
}
