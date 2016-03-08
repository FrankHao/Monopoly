using System.Collections;
using System.Collections.Generic;
using MiniJSON;

namespace Monopoly.Model
{

	[System.Serializable]
	public class Board {

		// holds all squares.
		List<Square> squares = new List<Square>();

		// constructor 
		public Board(string boardJSON)
		{
			List<object> boardObj = Json.Deserialize(boardJSON) as List<object>;

			int index = 0;
			foreach(object square in boardObj)
			{
				Dictionary<string, object> sqDict = square as Dictionary<string, object>;
				Square sqObj = new Square(index, sqDict);
				squares.Add(sqObj);
				index++;
			}
		}

		// TODO: check index is valid or not
		public Square GetSquareByIndex(int index)
		{
			return squares[index];
		}
	}
}

