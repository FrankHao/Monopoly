using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View {
	public class ConfirmDialog : MonoBehaviour {

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

		public void UpdateDialogInfo(string title, GenericCallBack cb)
		{
			titleText.text = title;
			callback = cb;
		}

	}
}

