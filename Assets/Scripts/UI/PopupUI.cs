﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly.View
{
    public class PopupUI : MonoBehaviour
    {

        public GameObject okBtn;
        public Text titleText;
        public delegate void GenericCallBack();
        protected GenericCallBack cb;

        public void UpdateInfo(string title, GenericCallBack callback)
        {
            titleText.text = title;
            cb = callback;
        }

        public virtual void OnClickOKBtn()
        {
            if (cb != null)
            {
                cb();
            }
            gameObject.SetActive(false);
        }
    }
}

