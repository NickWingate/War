using War.Domain.Entities;

namespace War.ConsoleUI.Services
{
	public interface IWinnerService
	{
		public Player DetermineWinner(Player player1, Player player2);
	}
}