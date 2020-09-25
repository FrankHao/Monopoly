using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monopoly.Model;

namespace Monopoly.View
{
    public class PropertyCardUI : PopupUI
    {
        public Image Banner;
        public Text SmallTitle;
        // public Text PropertyName; a.k.a Title
        public Text SinglRent;
        public List<RentRow> rentRows;
        public Text MortgageValue;
        public Text HouseCost;
        public Text HotelCost;
        public Text Footer;

        static readonly Dictionary<string, Color> ColorDict = new Dictionary<string, Color>()
        {
            {"red", Color.red },
            {"green", Color.green },
            {"blue", Color.blue },
            {"black", Color.black },
            {"pink", Color.magenta },
            {"white", Color.white },
            {"yellow", Color.yellow },
            {"orange", new Color(245/255,152/255,84/255) }
        };

        public void SetCard(Square square)
        {
            SmallTitle.text = "TITLE DEED";
            titleText.text = square.Name.ToUpper().Trim();
            SinglRent.text = "RENT $" + square.Rent;
            rentRows[0].ItemName.text = "WITH 1 HOUSE"; rentRows[0].Price.text = "$" + square.Rent1h.ToString();
            rentRows[1].ItemName.text = "WITH 2 HOUSE"; rentRows[1].Price.text = "$" + square.Rent2h.ToString();
            rentRows[2].ItemName.text = "WITH 3 HOUSE"; rentRows[2].Price.text = "$" + square.Rent3h.ToString();
            rentRows[3].ItemName.text = "WITH 4 HOUSE"; rentRows[3].Price.text = "$" + square.Rent4h.ToString();
            rentRows[4].ItemName.text = "WITH HOTEL"; rentRows[3].Price.text = "$" + square.RentHotel.ToString();
            MortgageValue.text = "MORTGAGE VALUE $" + square.Value / 2;
            HouseCost.text = $"HOUSES COST ${square.BuildCost} EACH";
            HotelCost.text = $"HOTELS {square.BuildCost} PLUS 4 HOUSES";
            Footer.text = "IF A PLAYER OWNS ALL THE LOTS OF ANY COLOR-GROUP, THEN RENT IS DOUBLED ON UNIMPROVED LOTS IN THAT GROUP.";
            if (ColorDict.ContainsKey(square.Color))
            {
                Banner.color = ColorDict[square.Color];
            }
        }
    }
}
