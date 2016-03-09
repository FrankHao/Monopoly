using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class PlayerPanel : MonoBehaviour {

		int playerIndex;
		public Image iconImage;
		public Text nameText;
		public Text cashText;

		public void InitPlayerInfo(int pIndex, string name, long cash)
		{
			playerIndex = pIndex;
			iconImage.sprite = Resources.Load<Sprite>(string.Format("Images/Players/{0}", playerIndex));
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

