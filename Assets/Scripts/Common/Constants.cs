using System;
using System.Collections;
using System.Collections.Generic;

namespace Monopoly.Common
{
	// for constant defines.
	public static class Constants
	{
		#region game setting
		public const int TOTAL_SQUARE_COUNT = 40;
		public const int PLAYER_COUNT = 2;
		public const int START_CASH = 1500;
		public const int GO_PASS_SALARY = 200;
		public const int GO_SQAURE_INDEX = 0;
		public const int SQUARE_COUNT_EACH_SIDE = 10;
		public const int NO_OWNER_INDEX = -1;
		#endregion

		#region square type
		public const string SQ_PROPERTY = "property";
		public const string SQ_STATION = "station";
		public const string SQ_UTITLITY = "utility";
		public const string SQ_JAIL = "jail";
		public const string SQ_GOTOJAIL = "gotojail";
		public const string SQ_CHANCE = "chance";
		public const string SQ_CHEST = "chest";
		public const string SQ_TAX = "tax";
		public const string SQ_FREE = "free";
		public const string SQ_GO = "go";
		#endregion

		#region game state
		public const int GAME_NOT_START = 0;
		public const int GAME_STARTED = 1;
		public const int GAME_LIMIT_ROUNDS = 5;
		#endregion

		#region dice number
		public const int DICE_COUNT = 2;
		public const int DICE_NUM_MIN = 1;
		public const int DICE_NUM_MAX = 6;
		#endregion

		#region rent
		public static readonly long[] STATION_RENT_PRICE = {25, 50, 100, 200};
		public static readonly long[] UTILITY_RENT = {4, 10};
		#endregion

		public const string HISTORY_ROLL = "ROLL";
		public const string HISTORY_BUY = "BUY";
		public const string HISTORY_RENT = "RENT";
	}
}