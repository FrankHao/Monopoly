using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Model
{

    [System.Serializable]
    public class Board
    {
        // holds all squares.
        List<Square> squares = new List<Square>();

        // constructor 
        public Board(string boardJSON)
        {

            int index = 0;
            squares = JsonConvert.DeserializeObject<List<Square>>(boardJSON);
            foreach (Square sq in squares)
            {
                sq.InitComplete(index);
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

