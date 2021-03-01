namespace War
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var game = new GameOfWar();
			game.Deal();
			game.Play();
		}
	}
}