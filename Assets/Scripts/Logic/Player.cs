using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Common;

namespace Monopoly.Model
{
	[System.Serializable]
	public class Player {

		#region properties
		public string Name {get; set;}
		public int PlayerIndex {get; set;}
		private int _cash;
		public int Cash {
			get {
				return _cash;
			}
			set {
				_cash = value;
				// trigger added cash event.
				if (addedCashEvent != null)
				{
					addedCashEvent(value, this);
				}
			}
		}

		private int _posIndex;
		public int PosIndex {
			get {
				return _posIndex;
			}
			set {
				_posIndex = value;

			}
		}
		#endregion

		#region events
		public delegate void initPlayer(Player player);
		public static event initPlayer initPlayerEvent;

		public delegate void movedPlayer(int playerIndex, int srcIndex, int tgtIndex);
		public static event movedPlayer movedPlayerEvent;

		public delegate void passGoSquare(Player player);
		public static event passGoSquare passGoSquareEvent;

		public delegate void addedCash(int cashDelta, Player player);
		public static event addedCash addedCashEvent;
		#endregion


		List<int> ownedSquares = new List<int>();

		public Player(int playerIndex, string name, int cash)
		{
			PosIndex = 0;
			PlayerIndex = playerIndex;
			Name = name;
			Cash = cash;

			// trigger init event
			if (initPlayerEvent != null)
			{
				initPlayerEvent(this);
			}
		}

		public void Move(int delta)
		{
			int srcIndex = PosIndex;
			int tgtIndex = PosIndex + delta;

			// this means pass or arrive GO square.
			if (tgtIndex >= LogicManager.SQUARE_COUNT)
			{
				// trigger GO event
				if (passGoSquareEvent != null)
				{
					passGoSquareEvent(this);
				}
				// set pos index
				PosIndex = tgtIndex - LogicManager.SQUARE_COUNT;
			}
			else
			{
				PosIndex += delta;
			}
				
			// trigger moved event
			if (movedPlayerEvent != null)
			{
				movedPlayerEvent(PlayerIndex, srcIndex, PosIndex);
			}
		}

		// set target index directly
		public void MoveTo(int target)
		{
			int srcIndex = PosIndex;
			if (target >= LogicManager.SQUARE_COUNT || target < 0)
			{
				Debug.LogError("target is invalid, " + target.ToString());
				return;
			}
			PosIndex = target;
		}

		public bool BuyProperty(Square square)
		{
			/*
			// add to owned squares, only keep index, easy to setup testcase.
			if (ownedSquares.Contains(square.PosIndex))
			{
				return false;
			}

			if (_cash < square.Value)
			{
				return false;
			}
			*/

			// TODO: check

			// change cash, trigger cash event
			//Cash -= square.Value;

			// add property to square list
			ownedSquares.Add(square.PosIndex);
			return true;
		}
	}
}

