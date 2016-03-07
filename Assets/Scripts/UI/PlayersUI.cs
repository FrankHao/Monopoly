using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Monopoly.View
{
	public class PlayersUI : MonoBehaviour {

		// index as key, no need to care about init sequence.
		Dictionary<int, GameObject> playerPanels = new Dictionary<int, GameObject>();

		// add player panels by player index.
		public void AddPlayerPanel(int playerIndex, string name, int cash)
		{
			if(playerPanels.ContainsKey(playerIndex))
			{
				UpdatePlayerPanel(playerIndex, name, cash);
			}
			else
			{
				// positions will be controlled by layout components
				GameObject playerPanel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayerPanel"));
				playerPanel.GetComponent<PlayerPanel>().UpdatePlayerInfo(playerIndex, name, cash);
				playerPanel.transform.SetParent(gameObject.transform, false);
				playerPanels.Add(playerIndex, playerPanel);
			}
		}

		public void UpdatePlayerPanel(int playerIndex, string name, int cash)
		{
			playerPanels[playerIndex].GetComponent<PlayerPanel>().UpdatePlayerInfo(playerIndex, name, cash);
		}
	}
}

