using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class EntryUI : MonoBehaviour {

		public delegate void gameStart();
		public static event gameStart gameStartEvent;


		public void OnClickPlayBtn()
		{
			gameObject.SetActive(false);
			if (gameStartEvent != null)
			{
				gameStartEvent();
			}
		}
	}
}


