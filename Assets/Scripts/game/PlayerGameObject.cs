using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Monopoly.Model;
using Monopoly.Common;

namespace Monopoly.Controller {
	public class PlayerGameObject : MonoBehaviour {

		// FIFO path
		Queue<KeyValuePair<int, Vector3>> movingPath;
		int endSquareIndex;
		Vector3 endPos;
		bool is_moving = false;
		float movingSpeed = 6f;

		private int _playerIndex;
		public int PlayerIndex {
			get {
				return _playerIndex;
			}
			set {
				_playerIndex = value;
				gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(string.Format("Images/Players/{0}", _playerIndex));
			}
		}

		public delegate void passGo(int playerIndex);
		public static event passGo passGoEvent;

		public delegate void movingEnd(int playerIndex, int squareIndex);
		public static event movingEnd movingEndEvent;

		void Update()
		{
			if (is_moving)
			{
				// move to the top position of path queue.
				float step = movingSpeed * Time.deltaTime;
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
				if (transform.localPosition == endPos)
				{
					// dequeue top position and move to.
					if (movingPath.Count > 0)
					{
						KeyValuePair<int, Vector3> kv = movingPath.Dequeue();
						endPos = kv.Value;
						endSquareIndex = kv.Key;

						// catch and trigger GO square event.
						if (kv.Key == Constants.GO_SQAURE_INDEX)
						{
							if (passGoEvent != null)
							{
								passGoEvent(PlayerIndex);
							}
						}
					}

					// no position to dequeue, means moved the end of path
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

			// check GO square
			if (kv.Key == Constants.GO_SQAURE_INDEX)
			{
				if (passGoEvent != null)
				{
					passGoEvent(PlayerIndex);
				}
			}
		}
	}
}
	