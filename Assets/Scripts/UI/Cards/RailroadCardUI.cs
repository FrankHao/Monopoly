using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monopoly.Model;
using Monopoly.Common;

namespace Monopoly.View
{
    public class RailroadCardUI : PopupUI
    {
        public Image Banner;
        public Text SmallTitle;
        // public Text PropertyName; a.k.a Title
        public Text SingleRent;
        public List<RentRow> rentRows;
        public Text MortgageValue;

        public void SetCard(Square square, GenericCallBack callback = null)
        {
            SmallTitle.text = square.Type.ToUpper().Trim();
            titleText.text = square.Name.ToUpper().Trim();
            rentRows[0].ItemName.text = "1 R.R. Owned"; rentRows[0].Price.text = "$" + square.Rent.ToString();
            rentRows[1].ItemName.text = "2 R.R. Owned"; rentRows[1].Price.text = "$" + square.Rent1h.ToString();
            rentRows[2].ItemName.text = "3 R.R. Owned"; rentRows[2].Price.text = "$" + square.Rent2h.ToString();
            rentRows[3].ItemName.text = "4 R.R. Owned"; rentRows[3].Price.text = "$" + square.Rent3h.ToString();
            MortgageValue.text = "MORTGAGE VALUE $" + square.Value / 2;
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
