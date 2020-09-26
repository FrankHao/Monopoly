using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
    public class PopupUI : MonoBehaviour
    {
        Button OKButton;
        public Text titleText;
        public delegate void GenericCallBack();
        protected GenericCallBack cb;

        private void Awake()
        {
            OKButton = GetComponentInChildren<Button>();
            OKButton.onClick.AddListener(OnClickOKBtn);
        }

        public void UpdateInfo(string title, GenericCallBack callback)
        {
            titleText.text = title;
            cb = callback;
        }

        protected virtual void OnClickOKBtn()
        {
            if (cb != null)
            {
                cb();
            }
            gameObject.SetActive(false);
        }
    }
}

