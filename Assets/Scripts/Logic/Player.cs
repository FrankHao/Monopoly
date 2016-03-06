using UnityEngine;
using System;
using System.Collections;

using Monopoly.Common;

namespace Monopoly.Model
{
	[System.Serializable]
	public class Player {

		public string Name {get; set;}
		public int PlayerIndex {get; set;}
		public int Cash {get; set;}
		public int PosIndex {get; set;}

		public delegate void initPlayer(Player player);
		public static event initPlayer initPlayerEvent;

		public Player(int playerIndex, string name, int cash)
		{
			PosIndex = 0;
			PlayerIndex = playerIndex;
			Name = name;
			Cash = cash;

			// trigger init event
			initPlayerEvent(this);
		}

		public void Move(int delta)
		{
			PosIndex += delta;
			if (PosIndex >= LogicManager.SQUARE_COUNT)
			{
				PosIndex = 0;
			}
		}

		public void MoveTo(int target)
		{
			if (target >= LogicManager.SQUARE_COUNT || target < 0)
			{
				Debug.LogError("target is invalid, " + target.ToString());
				return;
			}
			PosIndex = target;
		}
	}
}

