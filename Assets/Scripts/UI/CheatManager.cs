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

public class CheatWindow : EditorWindow, IHasCustomMenu
{
    // get the Preset icon and a style to display it
    private static class Styles
    {
        public static GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
        public static GUIStyle iconButton = new GUIStyle("IconButton");

    }

    [MenuItem("Agora/CheatWindow")]
    private static void ShowWindow()
    {
        GetWindow<CheatWindow>().Show();
    }

    // This interface implementation is automatically called by Unity.
    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
    {
        GUIContent content = new GUIContent("My Custom Entry");
        menu.AddItem(content, false, MyCallback);
    }

    private void MyCallback()
    {
        Debug.Log("My Callback was called.");
    }

    string Dice1Text = "1";
    string Dice2Text = "1";
    string JailFreeCard = "0";

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cheat Dice", EditorStyles.boldLabel);
        // create the Preset button at the end of the "MyManager Settings" line.
        // var buttonPosition = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, Styles.iconButton);

        Dice1Text = GUILayout.TextField(Dice1Text, 10);
        Dice2Text = GUILayout.TextField(Dice2Text, 10);

        if (GUILayout.Button("Click to set next roll"))
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"Cheat Dice: {Dice1Text} {Dice2Text}");
                CheatManager.instance.IsCheating = true;
                CheatManager.instance.dice1 = int.Parse(Dice1Text);
                CheatManager.instance.dice2 = int.Parse(Dice2Text);
                CheatManager.instance.CheatRollDice();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cheat BailFreeCards", EditorStyles.boldLabel);
        JailFreeCard = GUILayout.TextField(JailFreeCard, 10);
        if (GUILayout.Button("Set Number of Cards"))
        {
            if (Application.isPlaying)
            {
                int cards = int.Parse(JailFreeCard);
                CheatManager.instance.CheatSetFreeBailCard(cards);
            }
        }

        EditorGUILayout.EndHorizontal();

        // Draw the settings default Inspector and catch any change made to it.
        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck())
        {
            // Apply changes made in the settings editor to our instance.
            Debug.LogWarning("EndChangeCheck");
        }
    }
}