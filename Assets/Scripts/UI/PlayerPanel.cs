using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class PlayerPanel : MonoBehaviour {
		
		public Image iconImage;
		public Text nameText;
		public Text cashText;

		public void UpdatePlayerInfo(int playerIndex, string name, int cash)
		{
			iconImage.sprite = Resources.Load<Sprite>(string.Format("Images/Players/{0}", playerIndex));
			nameText.text = name;
			cashText.text = cash.ToString();
		}

		public void UpdateCash(int cash)
		{
			cashText.text = cash.ToString();
		}
	}
}

