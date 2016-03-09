using System.Collections;
using System.Collections.Generic;

using Monopoly.Common;

namespace Monopoly.Model
{	
	public class LogicManager {

		#region singleton
		// singleton
		private LogicManager() {}
		private static LogicManager _instance;
		public static LogicManager instance {
			get {
				if (_instance == null) {
					_instance = new LogicManager();
				}
				return _instance;
			}
		}
		#endregion
	
		Board board;
		List<Player> players = new List<Player>();
		public int State {get; set;}

		private int _currentPlayerIndex;
		public int CurrentPlayerIndex {
			get {
				return _currentPlayerIndex;
			}
			// trigger player index change event
			// to update UI highlight
			set {
				_currentPlayerIndex = value;
				if (changeTurnsEvent != null)
				{
					changeTurnsEvent(_currentPlayerIndex);
				}
			}
		}

		public delegate void GenericCallBack();

		public delegate void changeTurns(int playerIndex);
		public static event changeTurns changeTurnsEvent;

		// init game 
		public void GameStart(string boardJSON, GenericCallBack callback)
		{
			// set state to started.
			State = Constants.GAME_STARTED;

			// init board, squares
			board = new Board(boardJSON);

			// init players
			CurrentPlayerIndex = 0;
			for(int i=0; i<Constants.PLAYER_COUNT; i++)
			{
				string name = string.Format("player{0}", i);
				Player p = new Player(i, name, Constants.START_CASH);
				players.Add(p);
			}

			// finish initialization, then callback.
			if (callback != null)
			{
				callback();
			}
		}

		public void GameOver()
		{
			State = Constants.GAME_STARTED;
		}

		public int ChangeTurns()
		{
			int tmp = CurrentPlayerIndex + 1;
			CurrentPlayerIndex = tmp >= Constants.PLAYER_COUNT ? 0 : tmp;
			return CurrentPlayerIndex;
		}

		public int GetPlayerSquareIndex(int playerIndex)
		{
			return players[playerIndex].PosIndex;
		}

		public Square GetSquare(int squareIndex)
		{
			return board.GetSquareByIndex(squareIndex);
		}

		public long GetPlayerCash(int playerIndex)
		{
			return players[playerIndex].Cash;
		}

		public int GetSquareOwnerIndex(int squareIndex)
		{
			Square sq = board.GetSquareByIndex(squareIndex);
			return sq.OwnerIndex;
		}

		public long GetSquareValue(int squareIndex)
		{
			return board.GetSquareByIndex(squareIndex).Value;
		}

		public void MovePlayer(int delta)
		{
			Player player = players[CurrentPlayerIndex];
			player.Move(delta);
		}

		public long AddCashToPlayer(int playerIndex, long cashDelta)
		{
			players[playerIndex].Cash += cashDelta;
			return players[playerIndex].Cash;
		}

		public long SubCashFromPlayer(int playerIndex, long cashDelta)
		{
			players[playerIndex].Cash -= cashDelta;
			return players[playerIndex].Cash;
		}

		public void PlayerOwnSquare(int playerIndex, int squareIndex)
		{
			Player ply = instance.players[playerIndex];
			Square sq = instance.GetSquare(squareIndex);
			ply.OwnSquare(sq);
		}

		public bool IsAffordable(int playerIndex, int squareIndex)
		{
			Player ply = instance.players[playerIndex];
			long rentFee = instance.GetRentFee(playerIndex, squareIndex);
			return rentFee <= ply.GetTotalValue();
		}

		public long GetRentFee(int playerIndex, int squareIndex)
		{
			Square sq = instance.GetSquare(squareIndex);
			Player mover = instance.players[playerIndex];
			Player owner = instance.players[sq.OwnerIndex];

			// can be optimized with different sub classes of Square.
			if (sq.IsProperty())
			{
				return sq.GetMortgagePrice() - 20;
			}
			else if (sq.IsStation())
			{
				int ownedStationCount = owner.GetOwnedSquareCount(Constants.SQ_STATION);
				return Constants.STATION_RENT_PRICE[ownedStationCount-1];
			}
			else if (sq.IsUtility())
			{
				int ownedUtilityCount = owner.GetOwnedSquareCount(Constants.SQ_UTITLITY);
				return Constants.UTILITY_RENT[ownedUtilityCount-1] * mover.MovingDistance;
			}

			return 0;
		}

		public void PlayerBankrupt(int playerIndex)
		{
			Player ply = players[playerIndex];
			ply.Bankrupt();
		}

		public bool ReadyToGameOver()
		{
			int alivePlayerCount = 0;
			foreach(Player ply in players)
			{
				if (!ply.IsBankrupt)
				{
					alivePlayerCount++;
				}
			}
			return alivePlayerCount == 1;
		}



	}
}

