using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
	public class UIManager : MonoBehaviour {

		#region singleton
		// singleton design partten
		private UIManager() {}
		private static UIManager _instance;
		public static UIManager instance
		{   
			get 
			{   
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<UIManager>();
				}
				return _instance;
			}   
		}
		#endregion

		GameObject entryUI;
		GameObject rollingUI;
		GameObject playersUI;
		GameObject confirmUI;

		void Start()
		{
			InitUI();
		}
			
		void InitUI()
		{
			entryUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/EntryUI"));
			entryUI.transform.SetParent(gameObject.transform, false);

			rollingUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/RollingUI"));
			rollingUI.transform.SetParent(gameObject.transform, false);
			rollingUI.SetActive(false);

			playersUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayersUI"));
			playersUI.transform.SetParent(gameObject.transform, false);
			playersUI.SetActive(false);

			confirmUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConfirmUI"));
			confirmUI.transform.SetParent(gameObject.transform, false);
			confirmUI.SetActive(false);
		}
			
		public void UpdateDices(int[] nums)
		{
			rollingUI.GetComponent<RollingUI>().UpdateDices(nums[0], nums[1]);
		}

		public void ShowRollingUI()
		{
			rollingUI.SetActive(true);
		}

		public void ShowPlayersUI()
		{
			playersUI.SetActive(true);
		}

		public void ShowConfirmUI(ConfirmUI.GenericCallBack callback)
		{
			confirmUI.SetActive(true);
			confirmUI.GetComponent<ConfirmUI>().UpdateInfo("title", callback);
		}

		public void AddPlayerInfo(int playerIndex, string name, int cash)
		{
			playersUI.GetComponent<PlayersUI>().AddPlayerPanel(playerIndex, name, cash);
		}
	}

}
