using UnityEngine;
using UnityEditor;
using Monopoly.Controller;
using Monopoly.Model;

public class CheatManager
{
    public bool IsCheating { get; set; }
    public int dice1, dice2;

    static CheatManager _instance = null;
    private CheatManager()
    {
        dice1 = 1;
        dice2 = 1;
    }

    public static CheatManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CheatManager();
            }
            return _instance;
        }
    }

    public void Reset()
    {
        IsCheating = false;
    }

    public void CheatRollDice()
    {
        // get two dice numbers.
        int[] nums = new int[] { dice1, dice2 };

        // update UI
        // UIManager.instance.UpdateDices(nums);

        // move player in logic data, will move game object in event callback.
        LogicManager.instance.MovePlayer(nums);
    }

    public void CheatSetFreeBailCard(int count)
    {
        Player player = LogicManager.instance.GetPlayer(LogicManager.instance.CurrentPlayerIndex);
        Debug.LogWarning($"Setting Player {player.PlayerIndex}'s bail free card = {count}");
        player.JailBailCards = count;
    }
}
