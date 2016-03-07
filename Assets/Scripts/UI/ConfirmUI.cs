using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View {
	public class ConfirmUI : MonoBehaviour {

		public delegate void GenericCallBack();

		public Text titleText;
		public GenericCallBack callback = null;

		public void OnClickOKBtn()
		{
			if (callback != null)
			{
				callback();
			}
			gameObject.SetActive(false);
		}

		public void OnClickCancelBtn()
		{
			gameObject.SetActive(false);
		}

		public void UpdateInfo(string title, GenericCallBack cb)
		{
			titleText.text = title;
			callback = cb;
		}

	}
}

