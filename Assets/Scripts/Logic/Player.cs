using System;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Common;

namespace Monopoly.Model
{
	public class Player {

		#region properties
		public int PlayerIndex {get; set;}
		public string Name {get; set;}
		public long Cash {get; set;}
		public int PosIndex {get; set;}
		public int MovingDistance {get; set;}
		public bool IsBankrupt {get; set;}
		#endregion

		#region events
		public delegate void initPlayer(Player player);
		public static event initPlayer initPlayerEvent;

		public delegate void movedPlayer(int playerIndex, List<int> pathList);
		public static event movedPlayer movedPlayerEvent;

		public delegate void bankrupt(int playerIndex);
		public static event bankrupt bankruptEvent;
		#endregion

		// holds all owned squares
		List<Square> ownedSquares = new List<Square>();


		public Player(int playerIndex, string name, int cash)
		{
			PosIndex = 0;
			PlayerIndex = playerIndex;
			Name = name;
			Cash = cash;
			IsBankrupt = false;

			// trigger init event
			if (initPlayerEvent != null)
			{
				initPlayerEvent(this);
			}
		}

		public void Move(int[] nums)
		{
			// cache moving distance to calc rent fee.
			int delta = nums[0] + nums[1];
			MovingDistance = delta;

			// record end square index
			List<int> pathList = new List<int>();

			while(delta > 0)
			{
				delta--;
				PosIndex++;
				// pass corner
				if (PosIndex % Constants.SQUARE_COUNT_EACH_SIDE == 0)
				{
					// this is GO square
					if (PosIndex == Constants.TOTAL_SQUARE_COUNT)
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

		// own square and set relationship.
		public void OwnSquare(Square square)
		{
			ownedSquares.Add(square);
			square.OwnerIndex = PlayerIndex;
		}

		// get specific square type's count
		public int GetOwnedSquareCount(string sqType)
		{
			int count = 0;
			foreach(Square sq in ownedSquares)
			{
				if (sq.Type == sqType)
				{
					count++;
				}
			}
			return count;
		}

		// calc all properties + station + utitlity + cash
		public long GetTotalValue()
		{
			long totalValue = 0;
			foreach(Square sq in ownedSquares)
			{
				if (sq.IsBuyable())
				{
					totalValue += sq.GetMortgagePrice();
				}
			}
			totalValue += Cash;
			return totalValue;
		}

		// bankrupt
		public void Bankrupt()
		{
			IsBankrupt = true;
			if (bankruptEvent != null)
			{
				bankruptEvent(PlayerIndex);
			}
		}

		public List<int> GetOwnedSquareIndex()
		{
			List<int> sqs = new List<int>();
			foreach(Square sq in ownedSquares)
			{
				sqs.Add(sq.SquareIndex);
			}
			return sqs;
		}


	}
}

