﻿using System.Collections;
using System.Collections.Generic;

namespace Monopoly.Model
{
			
	public class LogicManager {

		#region constants
		public const int SQUARE_COUNT = 40;
		public const int PLAYER_COUNT = 2;
		public const int START_CASH = 1500;

		public const string SQ_PROPERTY = "property";
		public const string SQ_STATION = "station";
		public const string SQ_WATER = "water";
		public const string SQ_ELECTRIC = "electric";
		public const string SQ_JAIL = "jail";
		public const string SQ_GOTOJAIL = "gotojail";
		public const string SQ_CHANCE = "chance";
		public const string SQ_CHEST = "chest";
		public const string SQ_TAX = "tax";
		public const string SQ_FREE = "free";
		public const string SQ_GO = "go";
		#endregion

		#region properties
		public Board board {get; set;}
		List<Player> players = new List<Player>();
		int nextPlayerIndex = 0;
		#endregion

		public delegate void GenericCallBack(bool succ, string msg);

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

		// init game 
		public void InitGame(string boardJSON, GenericCallBack callback)
		{
			// init board, squares
			board = new Board(boardJSON);

			// init players
			nextPlayerIndex = 0;
			for(int i=0; i<PLAYER_COUNT; i++)
			{
				string name = string.Format("player{0}", i);
				Player p = new Player(i, name, START_CASH);
				players.Add(p);
			}

			// finish initialization, then callback.
			callback(true, "succ");
		}

		public int GenNextPlayerIndex()
		{
			nextPlayerIndex++;
			if (nextPlayerIndex >= PLAYER_COUNT)
			{
				nextPlayerIndex = 0;
			}
			return nextPlayerIndex;
		}

		public int GetNextPlayerIndex()
		{
			return nextPlayerIndex;
		}

		public void MovePlayer(int delta)
		{
			Player p = players[nextPlayerIndex];
			p.Move(delta);
		}

		public static int[] RollDice()
		{
			int num1 = UnityEngine.Random.Range(1, 7);
			int num2 = UnityEngine.Random.Range(1, 7);
			int[] nums = {num1, num2};
			return nums;
		}
	}
}
