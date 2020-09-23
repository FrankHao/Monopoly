using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
    public class PlayerPanel : MonoBehaviour
    {

        int playerIndex;
        public Image iconImage;
        public Text nameText;
        public Text cashText;

        static string[] PlayerIcons = {
            "mono-cat",
            "mono-dog",
            "mono-hat",
            "mono-spaceship",
            "mono-car",
            "mono-shoe",
            "mono-iron",
            "mono-sewneedle"
        };

        public static string GetPlayerSpriteName(int playerIndex)
        {
            return string.Format("Images/Players/{0}", PlayerIcons[playerIndex]);
        }

        public void InitPlayerInfo(int pIndex, string name, long cash)
        {
            playerIndex = pIndex;
            iconImage.sprite = Resources.Load<Sprite>(GetPlayerSpriteName(pIndex));
            nameText.text = name;
            cashText.text = cash.ToString();
            ShowHighlight(playerIndex == 0);
        }

        public void UpdateCash(long cash)
        {
            cashText.text = cash.ToString();
        }

        public void ShowHighlight(bool flag)
        {
            iconImage.color = flag ? Color.green : Color.white;
        }
    }
}

