namespace SquadronScramble;

internal static class Program
{
	private static void Main(string[] args)
	{
		using Dogfight dogfight = new Dogfight(args);
		dogfight.Run();
	}
}
