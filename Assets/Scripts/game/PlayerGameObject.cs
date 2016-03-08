using UnityEngine;
using System.Collections;

using Monopoly.Model;

namespace Monopoly.Controller {
	public class PlayerGameObject : MonoBehaviour {

		public int MovingDistance {get; set;}
		Vector3 startPos;
		Vector3 endPos;
		bool is_moving = false;
		float movingSpeed = 1f;

		public void UpdatePlayerInfo(string name, int cash)
		{

		}

		void Update()
		{
			if (is_moving)
			{
				float step = movingSpeed * Time.deltaTime;
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
			}
		}

		public void Move(Vector3 start, Vector3 end)
		{
			startPos = start;
			endPos = end;
			is_moving = true;
		}
	}
}
	