using System;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Common;
using Monopoly.View;

namespace Monopoly.Model
{
    public class Player
    {

        #region properties
        public int PlayerIndex { get; set; }
        public string Name { get; set; }
        public long Cash { get; set; }
        public int PosIndex { get; set; }
        public int MovingDistance { get; set; }
        public bool IsBankrupt { get; set; }

        // Jail
        public bool IsInJail { get { return JailTime > 0; } }

        // how many turns left in jail
        public int JailTime { get; set; }

        // number of Get out of Jail card owned
        public int JailBailCards { get; set; }
        #endregion

        #region events
        public delegate void initPlayer(Player player);
        public static event initPlayer initPlayerEvent;

        public delegate void movedPlayer(int playerIndex, List<int> pathList);
        public static event movedPlayer movedPlayerEvent;
        public delegate void jumpPlayer(int playerIndex, int squareIndex);
        public static event jumpPlayer movedToJailEvent;

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

            while (delta > 0)
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
            foreach (Square sq in ownedSquares)
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
            foreach (Square sq in ownedSquares)
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
            foreach (Square sq in ownedSquares)
            {
                sqs.Add(sq.SquareIndex);
            }
            return sqs;
        }

        public void EnterJail()
        {
            UnityEngine.Debug.Log($"Player {PlayerIndex} is going to jail.");
            JailTime = 3;
            movedToJailEvent?.Invoke(PlayerIndex, Constants.PRISON_SQUARE_INDEX);
            PosIndex = Constants.PRISON_SQUARE_INDEX;
        }

        public void ServeJailTime()
        {
            if (JailTime > 0) JailTime--;
        }
        /// <summary>
        ///   User bailing out of jail; if not enough cash player lost
        ///     return true if ok, false if bankrupt
        /// </summary>
        /// <returns></returns>
        public bool BailOutJail(long bail = Constants.BAIL_CASH)
        {
            JailTime = 0;
            Cash -= bail;
            UIManager.instance.UpdatePlayerCash(PlayerIndex, Cash);

            if (Cash < 0)
            {
                // TODO: if cash became negative, player lost
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UseJailCard(Action callbackOnOK, Action callBackOnCancel = null)
        {
            UIManager.instance.ShowConfirmUI($"Do you want to use the Get out of Jail card?\nYou still got {JailBailCards} left.",
                okCallBack: () =>
                {  // OK
                    JailBailCards--;
                    callbackOnOK();
                },
                cancelCallBack: callBackOnCancel
            );
        }

        public delegate void GenericCallBack();
    }
}

