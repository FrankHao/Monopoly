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
		public int PlayerIndex {get; set;}
		public string Name {get; set;}
		public long Cash {get; set;}
		public int PosIndex {get; set;}
		#endregion

		#region events
		public delegate void initPlayer(Player player);
		public static event initPlayer initPlayerEvent;

		public delegate void movedPlayer(int playerIndex, List<int> pathList);
		public static event movedPlayer movedPlayerEvent;
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
			// record end square index
			List<int> pathList = new List<int>();

			while(delta > 0)
			{
				delta--;
				PosIndex++;
				// pass corner
				if (PosIndex % LogicManager.SQUARE_COUNT_EACH_SIDE == 0)
				{
					// this is GO square
					if (PosIndex == LogicManager.TOTAL_SQUARE_COUNT)
					{
						PosIndex = 0;
					}
					pathList.Add(PosIndex);
				}
			}

			// record end square index
			pathList.Add(PosIndex);
				
			// trigger moved event
			if (movedPlayerEvent != null)
			{
				movedPlayerEvent(PlayerIndex, pathList);
			}
		}

		// set target index directly
		public void MoveTo(int target)
		{
			if (target >= LogicManager.TOTAL_SQUARE_COUNT || target < 0)
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

