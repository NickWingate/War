using System;
using System.Collections.Generic;
using War.ConsoleUI.Services;
using War.Domain.Entities;
using War.Domain.Enums;

namespace War.ConsoleUI
{
	public class GameOfWar
	{
		private readonly Func<string> _inputProvider;
		private readonly Action<string> _outputProvider;
		private readonly IWinnerService _winnerService;
		private Deck Deck;
		private Player Player1;
		private Player Player2;
		
		
		public GameOfWar(Func<string> inputProvider, Action<string> outputProvider)
		{
			_winnerService = new WinnerService();
			_inputProvider = inputProvider;
			_outputProvider = outputProvider;
			
			Deck = new Deck();
			Deck.ShuffleDeck();
			
			Player1 = CreatePlayer();
			Player2 = CreatePlayer();
			
			// Testing
			Player1.Hand.Insert(0, new Card(Suit.Clubs, 1));
			Player2.Hand.Insert(0, new Card(Suit.Clubs, 1));
			
		}

		public void Deal()
		{
			while (!Deck.IsEmpty)
			{
				Player1.Hand.Add(Deck.DrawCard());
				Player2.Hand.Add(Deck.DrawCard());
			}
		}

		public void Play()
		{
			Player roundWinner = null;
			var roundCards = new List<Card>();
			while (Player1.Hand.Count > 20 && Player2.Hand.Count > 20)
			{
				while (roundWinner == null)
				{
					roundCards.Add(Player1.DrawCard());
					roundCards.Add(Player2.DrawCard());
					roundWinner = _winnerService.DetermineWinner(Player1, Player2);
					_outputProvider($"{Player1} drew {Player1.CurrentCard}");
					_outputProvider($"{Player2} drew {Player2.CurrentCard}");
					if (roundWinner == null)
					{
						_outputProvider("There was a draw");
						GoToWar(roundCards);
					}
				}

				roundWinner.Score++;
				roundWinner.Hand.AddRange(roundCards);
				_outputProvider($"{roundWinner} won this round, they gained {roundCards.Count} cards and have " +
				                $"{roundWinner.Score} wins and {roundWinner.Hand.Count} cards in their hand");
				roundCards.Clear();
				roundWinner = null;
				_inputProvider();
			}

			var finalWinner = FindWinner(Player1, Player2);
			_outputProvider($"{finalWinner} won with a total of {finalWinner.Score} wins" +
			                $" and a hand of {finalWinner.Hand.Count} cards");
		}

		private void GoToWar(List<Card> roundCards)
		{
			roundCards.Add(Player1.DrawCard());
			roundCards.Add(Player1.DrawCard());
			roundCards.Add(Player1.DrawCard());
			
			roundCards.Add(Player2.DrawCard());
			roundCards.Add(Player2.DrawCard());
			roundCards.Add(Player2.DrawCard());
		}

		private Player FindWinner(Player player1, Player player2)
		{
			if (player1.Hand.Count > player2.Hand.Count)
			{
				return player1;
			}

			return player2;
		}

		private Player CreatePlayer()
		{
			_outputProvider("Player name: ");
			var playerName = _inputProvider() ?? string.Empty;
			var player = new Player(playerName);
			return player;
		}
	}
}