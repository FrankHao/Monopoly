using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class EntryUI : MonoBehaviour {

		public delegate void gameStart();
		public static event gameStart gameStartEvent;


		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void OnClickPlayBtn()
		{
			gameObject.SetActive(false);
			gameStartEvent();
		}
	}
}


