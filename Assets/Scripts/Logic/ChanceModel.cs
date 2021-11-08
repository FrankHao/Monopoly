using System;
using System.Collections.Generic;

namespace Monopoly.Model
{
    [Serializable]
    public class ChanceModel
    {
        public string Description { get; set; }
        public string Type { get; set; }
        // for pay it is int, for mpay, format "M:N"
        public string Amount { get; set; } 
        public string From { get; set; }
        public string To { get; set; }

        // Chance vs Community (box)
        public string Box { get; set; }


        public override string ToString()
        {
            return ($"Description:{Description} Type:{Type} Amount:{Amount} From:{From} To:{To}");
        }
    }
}
