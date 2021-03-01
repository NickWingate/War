using System;

namespace War.ConsoleUI
{
	class Program
	{
		static void Main(string[] args)
		{
			var game = new GameOfWar(Console.ReadLine, Console.WriteLine);
			game.Deal();
			game.Play();
		}
	}
}