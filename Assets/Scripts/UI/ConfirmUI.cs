using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View {
	public class ConfirmUI : MonoBehaviour {

		public delegate void GenericCallBack();

		public Text titleText;
		public GenericCallBack okCallBack = null;
		public GenericCallBack cancelCallBack = null;

		public void OnClickOKBtn()
		{
			if (okCallBack != null)
			{
				okCallBack();
			}
			gameObject.SetActive(false);
		}

		public void OnClickCancelBtn()
		{
			if (cancelCallBack != null)
			{
				cancelCallBack();
			}
			gameObject.SetActive(false);
		}

		public void UpdateInfo(string title, GenericCallBack okCB, GenericCallBack cancelCB)
		{
			titleText.text = title;
			okCallBack = okCB;
			cancelCallBack = cancelCB;
		}

	}
}

