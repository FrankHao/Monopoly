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

        [SerializeField]
        long currentCash;

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
            currentCash = cash;
            cashText.text = cash.ToString();
            ShowHighlight(playerIndex == 0);
        }

        public void UpdateCash(long cash)
        {
            NumberRoller numberRoller = cashText.gameObject.GetComponent<NumberRoller>();
            if (numberRoller == null)
            {
                numberRoller = cashText.gameObject.AddComponent<NumberRoller>();
            }
            numberRoller.enabled = false;
            numberRoller.Setup(currentCash, cash, 1f, () =>
            {
                currentCash = cash;
                numberRoller.enabled = false;
            });
            numberRoller.enabled = true;
        }

        public void ShowHighlight(bool flag)
        {
            iconImage.color = flag ? Color.green : Color.white;
        }
    }
}

