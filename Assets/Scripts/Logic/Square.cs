using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Monopoly.Model
{
	// Holds square value
	[System.Serializable]
	public class Square {

		#region properties
		public string Name {get;set;}
		public string Desc {get;set;}
		public long Value {get;set;}
		public string Color {get;set;}
		public string Type {get;set;}
		#endregion

		public delegate void initSquare(Square squareObj, int index);
		public static event initSquare initSquareEvent;

		public Square(int index, Dictionary<string, object> sqDict)
		{
			Name = (string)sqDict["name"];
			Desc = (string)sqDict["desc"];
			Type = (string)sqDict["type"];
			Color = (string)sqDict["color"];
			Value = (long)sqDict["value"];

			// trigger square event to create game object.
			initSquareEvent(this, index);
		}
	}
}

