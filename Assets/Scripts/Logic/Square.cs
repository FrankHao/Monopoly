using System;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Common;
using Monopoly.Controller;

namespace Monopoly.Model
{
    // Holds square value
    [Serializable]
    public class Square
    {
        public int SquareIndex { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public long Value { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public int Rent { get; set; }
        public int Rent1h { get; set; }
        public int Rent2h { get; set; }
        public int Rent3h { get; set; }
        public int Rent4h { get; set; }
        public int RentHotel { get; set; }
        public int BuildCost { get; set; }

        int _ownerIndex = Constants.NO_OWNER_INDEX;
        public int OwnerIndex
        {
            get
            {
                return _ownerIndex;
            }
            set
            {
                _ownerIndex = value;
                AssignOwner(_ownerIndex);
            }
        }
        public delegate void initSquare(Square square);
        public static event initSquare initSquareEvent;

        public Square(int index, Dictionary<string, object> sqDict)
        {
            SquareIndex = index;
            try
            {
                Name = (string)sqDict["name"];
                Desc = (string)sqDict["desc"];
                Type = (string)sqDict["type"];
                Color = (string)sqDict["color"];
                Value = (long)sqDict["value"];

                Rent = (int)sqDict["Rent"];
                Rent1h = (int)sqDict["Rent1h"];
                Rent2h = (int)sqDict["Rent2h"];
                Rent3h = (int)sqDict["Rent3h"];
                Rent4h = (int)sqDict["Rent4h"];
                RentHotel = (int)sqDict["RentHotel"];
                BuildCost = (int)sqDict["Build"];
            }
            catch
            {

            }

            // trigger square event to create game object.
            if (initSquareEvent != null)
            {
                initSquareEvent(this);
            }
        }

        public long GetRentPrice(string squareType, int diceNum)
        {
            long rentFee = 0;

            switch (squareType)
            {
                // normal property
                case Constants.SQ_PROPERTY:
                    rentFee = GetMortgagePrice() - 20;
                    break;

                // station rent 
                case Constants.SQ_STATION:
                    Random rnd = new Random();
                    rentFee = Constants.STATION_RENT_PRICE[rnd.Next(Constants.STATION_RENT_PRICE.Length)];
                    break;

                // water or electric
                case Constants.SQ_UTITLITY:
                    Random rnd2 = new Random();
                    rentFee = rnd2.Next(0, 2) == 0 ? 4 * diceNum : 10 * diceNum;
                    break;

                default:
                    break;
            }
            return rentFee;
        }

        public long GetMortgagePrice()
        {
            return Value / 2;
        }

        public bool IsOwned()
        {
            return OwnerIndex != Constants.NO_OWNER_INDEX;
        }

        public bool IsProperty()
        {
            return Type == Constants.SQ_PROPERTY;
        }

        public bool IsStation()
        {
            return Type == Constants.SQ_STATION;
        }

        public bool IsUtility()
        {
            return Type == Constants.SQ_UTITLITY;
        }

        public bool IsBuyable()
        {
            return Type == Constants.SQ_PROPERTY || Type == Constants.SQ_STATION || Type == Constants.SQ_UTITLITY;
        }

        private void AssignOwner(int ownIdx)
        {
            SquareGameObject sgo = GameManager.instance.SquareGameObjects[SquareIndex].GetComponent<SquareGameObject>();
            sgo?.AssignOwner(ownIdx);
        }
    }
}

