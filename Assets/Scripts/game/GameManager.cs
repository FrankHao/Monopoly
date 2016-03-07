using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Monopoly.View;
using Monopoly.Model;

namespace Monopoly.Controller
{
	public class GameManager : MonoBehaviour {

		#region singleton
		// singleton design partten
		private GameManager() {}
		private static GameManager _instance;
		public static GameManager instance
		{   
			get 
			{   
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<GameManager>();
				}
				return _instance;
			}   
		}
		#endregion

		GameObject boardGameObj;
		List<GameObject> squareGameObjs = new List<GameObject>();
		Dictionary<int, GameObject> playerGameObjs = new Dictionary<int, GameObject>();

		void Start() 
		{
			// view event handlers
			EntryUI.gameStartEvent += EntryUI_gameStartEvent;	
			RollingUI.rollDiceEvent += RollingUI_rollDiceEvent;

			// model event handlers
			Square.initSquareEvent += Square_initSquareEvent;
			Player.initPlayerEvent += Player_initPlayerEvent;
		}

		void Player_initPlayerEvent (Player player)
		{
			// create player game object
			GameObject playerObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Player"));
			playerObj.transform.SetParent(boardGameObj.transform);
			playerObj.GetComponent<PlayerGameObject>().UpdatePlayerInfo(player.Name, player.Cash);
			playerGameObjs.Add(player.PlayerIndex, playerObj);

			// fill player UI object
			UIManager.instance.AddPlayerInfo(player.PlayerIndex, player.Name, player.Cash);
		}

		// release all events, resources, reference.
		void OnDestroy()
		{
			EntryUI.gameStartEvent -= EntryUI_gameStartEvent;
			RollingUI.rollDiceEvent -= RollingUI_rollDiceEvent;
			Square.initSquareEvent -= Square_initSquareEvent;

			Destroy(boardGameObj);
		}

		void EntryUI_gameStartEvent ()
		{
			instance.GameStart();
		}

		void InitGameCallBack(bool succ, string msg)
		{
			// when everything is ready, show rolling UI
			UIManager.instance.ShowRollingUI();
		}

		public void GameStart() 
		{
			// init board game object.
			InitBoard();

			// load board JSON file as TextAsset.
			TextAsset boardText = Resources.Load<TextAsset>("JSON/board");

			// init logic data
			// square game objects will be created in callback event.
			LogicManager.instance.InitGame(boardText.text, InitGameCallBack);
		}

		void InitBoard()
		{
			boardGameObj = Instantiate(Resources.Load<GameObject>("prefabs/game/Board"));
			boardGameObj.transform.SetParent(gameObject.transform);
			Debug.Log("bounds is " + boardGameObj.GetComponent<SpriteRenderer>().bounds);
			Debug.Log("bounds is " + boardGameObj.GetComponent<SpriteRenderer>().sprite.bounds);
			//boardGameObj.GetComponent<SpriteRenderer>().bounds
		}

		// use square data to init game object.
		void Square_initSquareEvent (Square squareObj, int index)
		{
			GameObject square = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Square"));

			// corner index
			if (index == 0)
			{
				square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/corner_square");
			}
			square.transform.SetParent(boardGameObj.transform);

			// pace squares on board.
			float offsetX = 0f;
			float offsetY = 0f;
			float rotationZ = 0f;

			if (index > 0)
			{
				GameObject prevSquare = squareGameObjs[index - 1];
				Vector3 prevBoundSize = prevSquare.GetComponent<SpriteRenderer>().bounds.size;
				if (index > 0 && index <= 10)
				{
					square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/normal_square_lay");
					if (index == 10)
					{
						square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/corner_square");
					}
					offsetY = prevBoundSize.y;
				}
				else if( index >= 11 && index <= 20 )
				{
					if (index == 20)
					{
						square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/corner_square");
					}
					offsetX = prevBoundSize.x;
				}
				else if( index >= 21 && index <= 30 )
				{
					square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/normal_square_lay");
					if (index == 30)
					{
						square.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Board/corner_square");
					}
					offsetY = -square.GetComponent<SpriteRenderer>().bounds.size.y;
				}
				else if( index >= 31 && index < 40 )
				{
					offsetX = -square.GetComponent<SpriteRenderer>().bounds.size.x;
				}

				square.transform.localPosition = new Vector3(prevSquare.transform.localPosition.x + offsetX, 
				                                             prevSquare.transform.localPosition.y + offsetY, 
				                                             0f);
			}
			else
			{
				square.transform.localPosition = new Vector3(0f, 0f, 0f);
			}



			// add to square game objects list, easy to access.
			squareGameObjs.Add(square);

			Debug.Log("local pos" + square.transform.localPosition);
			//square.transform.localPosition = square.transform.localPosition + size;


		}

		// after click dice
		void RollingUI_rollDiceEvent ()
		{
			// get numbers from logic manager.
			int[] nums = LogicManager.RollDice();

			// update UI
			UIManager.instance.UpdateDices(nums);

			// move logic player
			int delta = nums[0] + nums[1];
			LogicManager.instance.MovePlayer(delta);

			// move game player
			int playerIndex = LogicManager.instance.GetCurrentPlayerIndex();
			//playerGameObjs[playerIndex].GetComponent<PlayerGameObject>().Move(delta);

		}

	}
}


