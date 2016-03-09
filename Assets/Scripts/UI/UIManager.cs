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
				// potentially if we have multi scenes
				// need to take care of duplicated issue, when switch between different scenes.
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<UIManager>();
					DontDestroyOnLoad(_instance.gameObject);
				}
				return _instance;
			}   
		}
		#endregion

		GameObject entryUI;
		GameObject rollingUI;
		GameObject playersUI;
		GameObject confirmUI;
		GameObject popupUI;

		void Awake()
		{
			InitUI();
		}
			
		void InitUI()
		{
			instance.entryUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/EntryUI"));
			instance.entryUI.transform.SetParent(gameObject.transform, false);

			instance.rollingUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/RollingUI"));
			instance.rollingUI.transform.SetParent(gameObject.transform, false);
			instance.rollingUI.SetActive(false);

			instance.playersUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayersUI"));
			instance.playersUI.transform.SetParent(gameObject.transform, false);
			instance.playersUI.SetActive(false);

			instance.confirmUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConfirmUI"));
			instance.confirmUI.transform.SetParent(gameObject.transform, false);
			instance.confirmUI.SetActive(false);

			instance.popupUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PopupUI"));
			instance.popupUI.transform.SetParent(gameObject.transform, false);
			instance.popupUI.SetActive(false);

		}
			
		public void UpdateDices(int[] nums)
		{
			instance.rollingUI.GetComponent<RollingUI>().UpdateDices(nums[0], nums[1]);
		}

		public void ShowRollingUI()
		{
			instance.rollingUI.SetActive(true);
		}

		public void ShowPlayersUI()
		{
			instance.playersUI.SetActive(true);
		}

		public void ShowPopupUI(string title, PopupUI.GenericCallBack callback)
		{
			instance.popupUI.GetComponent<PopupUI>().UpdateInfo(title, callback);
			instance.popupUI.SetActive(true);
		}

		public void ShowConfirmUI(string title, 
			ConfirmUI.GenericCallBack okCallBack, 
			ConfirmUI.GenericCallBack cancelCallBack)
		{
			instance.confirmUI.SetActive(true);
			instance.confirmUI.GetComponent<ConfirmUI>().UpdateInfo(title, okCallBack, cancelCallBack);
		}

		public void AddPlayerInfo(int playerIndex, string name, long cash)
		{
			instance.playersUI.GetComponent<PlayersUI>().AddPlayerPanel(playerIndex, name, cash);
		}

		public void UpdatePlayerCash(int playerIndex, long cash)
		{
			instance.playersUI.GetComponent<PlayersUI>().UpdatePlayerCash(playerIndex, cash);
		}

		public void UpdateCurrentPlayerIndex(int playerIndex)
		{
			instance.playersUI.GetComponent<PlayersUI>().UpdateCurrentPlayerIndex(playerIndex);
			instance.rollingUI.GetComponent<RollingUI>().EnableRollButton();
		}
	}

}
