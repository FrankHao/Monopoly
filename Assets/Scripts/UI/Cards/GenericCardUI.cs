using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monopoly.Model;
using Monopoly.Common;

namespace Monopoly.View
{
    public class GenericCardUI : PopupUI
    {
        public Image Banner;
        public Text SmallTitle;
        public Text MainDescription;
        public Text FootNote;

        public void SetCard(Square square, GenericCallBack callback = null)
        {
            SmallTitle.text = square.Type.ToUpper().Trim();
            titleText.text = square.Name.ToUpper().Trim();
            MainDescription.text = square.Desc;
            FootNote.text = "MORTGAGE VALUE $" + square.Value / 2;
            if (Constants.ColorDict.ContainsKey(square.Color))
            {
                Banner.color = Constants.ColorDict[square.Color];
            }
        }

        protected override void OnClickOKBtn()
        {
            if (cb != null)
            {
                cb();
            }
            GameObject.Destroy(gameObject);
        }
    }
}