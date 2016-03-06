using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class RollingUI : MonoBehaviour {

		public GameObject dice1;
		public GameObject dice2;

		public delegate void rollDice();
		public static event rollDice rollDiceEvent;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void UpdateDices(int num1, int num2)
		{
			//Debug.Log("num1 " + num1 + " " + string.Format("Images/dice_{0}", num1-1));
			dice1.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Images/NO_{0}", num1));
			dice2.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Images/NO_{0}", num2));
		}

		public void OnClickRollBtn()
		{
			if (rollDiceEvent != null)
			{
				rollDiceEvent();
			}
		}
	}
}

