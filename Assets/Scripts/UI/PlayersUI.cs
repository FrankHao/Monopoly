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
		public void AddPlayerPanel(int playerIndex, string name, long cash)
		{
			if(!playerPanels.ContainsKey(playerIndex))
			{
				// positions will be controlled by layout components
				GameObject playerPanel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayerPanel"));
				playerPanel.GetComponent<PlayerPanel>().InitPlayerInfo(playerIndex, name, cash);
				playerPanel.transform.SetParent(gameObject.transform, false);
				playerPanels.Add(playerIndex, playerPanel);
			}
		}

		public void UpdatePlayerCash(int playerIndex, long cash)
		{
			playerPanels[playerIndex].GetComponent<PlayerPanel>().UpdateCash(cash);
		}

		// highlight current player panel
		public void UpdateCurrentPlayerIndex(int playerIndex)
		{
			foreach(KeyValuePair<int, GameObject> kv in playerPanels)
			{
				kv.Value.GetComponent<PlayerPanel>().ShowHighlight(kv.Key == playerIndex);
			}
		}
	}
}

