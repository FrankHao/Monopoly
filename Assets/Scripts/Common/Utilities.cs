using System;
namespace Monopoly.Common
{
    // for some util functions
    public static class Utilities
    {
        public static int[] GetTwoDiceNumbers()
        {
            Random rnd = new Random();
            int num1 = CheatManager.instance.IsCheating ? CheatManager.instance.dice1 : rnd.Next(Constants.DICE_NUM_MIN, Constants.DICE_NUM_MAX + 1);
            int num2 = CheatManager.instance.IsCheating ? CheatManager.instance.dice2 : rnd.Next(Constants.DICE_NUM_MIN, Constants.DICE_NUM_MAX + 1);
            int[] nums = { num1, num2 };

            if (CheatManager.instance.IsCheating) { CheatManager.instance.Reset(); }
            return nums;
        }
    }
}

