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

		List<List<string>> playHistory = new List<List<string>>();

		public int State {get; set;}
		public int PlayedRounds {get; set;}

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

		public delegate void gameStart();
		public static event gameStart gameStartEvent;

		public delegate void gameOver();
		public static event gameOver gameOverEvent;

		// init game 
		public void GameStart(string boardJSON)
		{			
			// set state to started.
			instance.State = Constants.GAME_STARTED;

			// set limit rounds
			instance.PlayedRounds = 0;

			// init board, squares
			instance.board = new Board(boardJSON);

			// init players
			instance.CurrentPlayerIndex = 0;
			for(int i=0; i<Constants.PLAYER_COUNT; i++)
			{
				string name = string.Format("player{0}", i);
				Player p = new Player(i, name, Constants.START_CASH);
				instance.players.Add(p);
			}

			if (gameStartEvent != null)
			{
				gameStartEvent();
			}
		}

		public void GameOver()
		{
			instance.State = Constants.GAME_NOT_START;
			if (gameOverEvent != null)
			{
				gameOverEvent();
			}
		}

		public void Reset()
		{
			instance.board = null;
			instance.players.Clear();
			instance.State = Constants.GAME_NOT_START;
			instance.PlayedRounds = 0;
			instance.CurrentPlayerIndex = 0;
			instance.playHistory.Clear();
		}

		public int ChangeTurns()
		{
			int tmp = instance.CurrentPlayerIndex + 1;
			instance.CurrentPlayerIndex = tmp >= Constants.PLAYER_COUNT ? 0 : tmp;
			// back to 0 is a new round
			if (instance.CurrentPlayerIndex == 0)
			{
				instance.PlayedRounds++;
				// FOR TEST : reach limit rounds to end game.
				if (instance.PlayedRounds >= Constants.GAME_LIMIT_ROUNDS)
				{
					instance.GameOver();
				}
			}
			return instance.CurrentPlayerIndex;
		}

		public int GetPlayerSquareIndex(int playerIndex)
		{
			return instance.players[playerIndex].PosIndex;
		}

		public Square GetSquare(int squareIndex)
		{
			return instance.board.GetSquareByIndex(squareIndex);
		}

		public long GetPlayerCash(int playerIndex)
		{
			return instance.players[playerIndex].Cash;
		}

		public int GetSquareOwnerIndex(int squareIndex)
		{
			Square sq = instance.board.GetSquareByIndex(squareIndex);
			return sq.OwnerIndex;
		}

		public long GetSquareValue(int squareIndex)
		{
			return instance.board.GetSquareByIndex(squareIndex).Value;
		}

		public void MovePlayer(int[] nums)
		{
			Player player = instance.players[instance.CurrentPlayerIndex];
			player.Move(nums);
			instance.RecordRollHistory(instance.CurrentPlayerIndex, nums);
		}

		public void RecordRollHistory(int playerIndex, int[] nums)
		{
			// record roll history
			List<string> record = new List<string>();
			record.Add(Constants.HISTORY_ROLL);
			record.Add(instance.CurrentPlayerIndex.ToString());
			record.Add(nums[0].ToString());
			record.Add(nums[1].ToString());
			instance.playHistory.Add(record);
		}

		public void RecordPlayHistory(string historyType, int playerIndex, int squareIndex, long cost, long cash)
		{
			List<string> record = new List<string>();
			record.Add(historyType);
			record.Add(playerIndex.ToString());
			record.Add(squareIndex.ToString());
			record.Add(cost.ToString());
			record.Add(cash.ToString());
			instance.playHistory.Add(record);
		}
			
		public long AddCashToPlayer(int playerIndex, long cashDelta)
		{
			instance.players[playerIndex].Cash += cashDelta;
			return instance.players[playerIndex].Cash;
		}

		public long SubCashFromPlayer(int playerIndex, long cashDelta)
		{
			instance.players[playerIndex].Cash -= cashDelta;
			return instance.players[playerIndex].Cash;
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
			Player ply = instance.players[playerIndex];
			ply.Bankrupt();
		}

		public bool ReadyToGameOver()
		{
			int alivePlayerCount = 0;
			foreach(Player ply in instance.players)
			{
				if (!ply.IsBankrupt)
				{
					alivePlayerCount++;
				}
			}
			return alivePlayerCount == 1;
		}

		public bool IsGameOver()
		{
			return instance.State == Constants.GAME_NOT_START;
		}

		public List<List<string>> GetPlayHistory()
		{
			return instance.playHistory;
		}

		public List<Player> GetPlayers()
		{
			return instance.players;
		}

	}
}

