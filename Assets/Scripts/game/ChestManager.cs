using UnityEngine;
using System.Collections.Generic;
using Monopoly.Model;
using Newtonsoft.Json;
namespace Monopoly.Controller
{
    public class ChestManager 
    {
        List<ChanceModel> Chances;
        List<ChanceModel> CommunityChests;

        public ChestManager()
        {
            Chances = new List<ChanceModel>();
            CommunityChests = new List<ChanceModel>();
	    }

        public void LoadData()
        {
            // load board JSON file as TextAsset.
            TextAsset boardText = Resources.Load<TextAsset>("JSON/chances");
            List<ChanceModel> models = JsonConvert.DeserializeObject<List<ChanceModel>>(boardText.text);
            foreach (ChanceModel m in models)
            { 
                Debug.Log(m.ToString());
	            if (m.Box == "chance") {
                    Chances.Add(m);
		        } else if (m.Box == "community") {
                    CommunityChests.Add(m);
		        }
	        }

        }

    }

}
