using System;
using System.Collections;

namespace Monopoly.Common
{
	// for some util functions
	public static class Utilities
	{
		public static int[] GetTwoDiceNumbers()
		{
			Random rnd = new Random();
			int num1 = rnd.Next(Constants.DICE_NUM_MIN, Constants.DICE_NUM_MAX+1);
			int num2 = rnd.Next(Constants.DICE_NUM_MIN, Constants.DICE_NUM_MAX+1);
			int[] nums = {num1, num2};
			return nums;
		}
	}
}

