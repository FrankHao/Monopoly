using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Model;

namespace Monopoly.Controller {
	public class PlayerGameObject : MonoBehaviour {

		Queue<KeyValuePair<int, Vector3>> movingPath;
		int endSquareIndex;
		Vector3 endPos;
		bool is_moving = false;
		float movingSpeed = 3f;

		public int PlayerIndex{get; set;}

		public delegate void passGo(int playerIndex);
		public static event passGo passGoEvent;

		public delegate void movingEnd(int playerIndex, int squareIndex);
		public static event movingEnd movingEndEvent;

		void Update()
		{
			if (is_moving)
			{
				float step = movingSpeed * Time.deltaTime;
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
				if (transform.localPosition == endPos)
				{
					if (movingPath.Count > 0)
					{
						KeyValuePair<int, Vector3> kv = movingPath.Dequeue();
						endPos = kv.Value;
						endSquareIndex = kv.Key;
						if (kv.Key == LogicManager.GO_SQAURE_INDEX)
						{
							if (passGoEvent != null)
							{
								passGoEvent(PlayerIndex);
							}
						}
					}
					else
					{
						if (movingEndEvent != null)
						{
							movingEndEvent(PlayerIndex, endSquareIndex);
						}
						is_moving = false;
					}
				}
			}
		}

		public void Move(Queue<KeyValuePair<int, Vector3>> pathQueue)
		{
			movingPath = pathQueue;
			KeyValuePair<int, Vector3> kv = pathQueue.Dequeue();
			endPos = kv.Value;
			endSquareIndex = kv.Key;
			is_moving = true;

			if (kv.Key == LogicManager.GO_SQAURE_INDEX)
			{
				if (passGoEvent != null)
				{
					passGoEvent(PlayerIndex);
				}
			}
		}
	}
}
	