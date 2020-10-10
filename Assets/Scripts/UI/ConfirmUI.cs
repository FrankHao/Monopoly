using UnityEngine;
using UnityEngine.UI;
using System;

namespace Monopoly.View
{
    public class ConfirmUI : MonoBehaviour
    {

        public Text titleText;
        public Action okCallBack = null;
        public Action cancelCallBack = null;

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

        public void UpdateInfo(string title, Action okCB, Action cancelCB)
        {
            titleText.text = title;
            okCallBack = okCB;
            cancelCallBack = cancelCB;
        }

    }
}

