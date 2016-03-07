using UnityEngine;
using System.Collections;

using Monopoly.Model;

namespace Monopoly.Controller {
	public class PlayerGameObject : MonoBehaviour {

		public void UpdatePlayerInfo(string name, int cash)
		{

		}

		public void Move(int delta)
		{
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}
}
	