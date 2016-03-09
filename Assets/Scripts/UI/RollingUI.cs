using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class RollingUI : MonoBehaviour {

		public GameObject rollBtn;
		public GameObject autoBtn;
		public GameObject dice1;
		public GameObject dice2;

		public delegate void rollDice();
		public static event rollDice rollDiceEvent;

		public delegate void autoMode();
		public static event autoMode autoModeEvent;

		public void UpdateDices(int num1, int num2)
		{
			dice1.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Images/Dices/NO_{0}", num1));
			dice2.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Images/Dices/NO_{0}", num2));
		}

		public void OnClickRollBtn()
		{	
			if (rollDiceEvent != null)
			{
				rollDiceEvent();
			}
			// disable button, wait until this turn finish.
			rollBtn.GetComponent<Button>().enabled = false;
		}

		public void EnableRollButton()
		{
			rollBtn.GetComponent<Button>().enabled = true;
		}

		public void OnClickAutoBtn()
		{
			string curText = autoBtn.GetComponentInChildren<Text>().text;
			autoBtn.GetComponentInChildren<Text>().text = (curText == "Auto") ? "Manual" : "Auto";

			if (autoModeEvent != null)
			{
				autoModeEvent();
			}

			if (autoBtn.GetComponentInChildren<Text>().text == "Manual")
			{
				OnClickRollBtn();
				rollBtn.SetActive(false);
			}
			else
			{
				rollBtn.SetActive(true);
			}
		}
	}
}

