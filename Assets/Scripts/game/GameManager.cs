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
			Player.movedPlayerEvent += Player_movedPlayerEvent;
		}

		// moved player event handler
		void Player_movedPlayerEvent(int playerIndex, int srcIndex, int tgtIndex)
		{
			MovePlayer(playerIndex, srcIndex, tgtIndex);
		}

		// get the center position on specific square, as square pivot is bottom left.
		Vector3 GetCenterPositionOnSquare(int squareIndex)
		{
			GameObject squareObj = squareGameObjs[squareIndex];
			Vector3 pos = squareObj.transform.localPosition;
			Vector3 extents = squareObj.GetComponent<SpriteRenderer>().bounds.extents;
			return new Vector3(pos.x + extents.x, pos.y + extents.y, 0f);
		}

		void Player_initPlayerEvent (Player player)
		{
			// create player game object
			GameObject playerObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Player"));
			playerObj.transform.SetParent(boardGameObj.transform);
			playerObj.GetComponent<PlayerGameObject>().UpdatePlayerInfo(player.Name, player.Cash);
			playerObj.transform.localPosition = GetCenterPositionOnSquare(0);
			playerGameObjs.Add(player.PlayerIndex, playerObj);

			// create player UI object
			UIManager.instance.AddPlayerInfo(player.PlayerIndex, player.Name, player.Cash);
		}

		// release all events, resources, reference.
		void OnDestroy()
		{
			EntryUI.gameStartEvent -= EntryUI_gameStartEvent;
			RollingUI.rollDiceEvent -= RollingUI_rollDiceEvent;
			Square.initSquareEvent -= Square_initSquareEvent;
			Player.initPlayerEvent -= Player_initPlayerEvent;
			Player.movedPlayerEvent -= Player_movedPlayerEvent;

			Destroy(boardGameObj);
		}

		void EntryUI_gameStartEvent ()
		{
			GameStart();
		}

		void InitGameCallBack()
		{
			// when everything is ready, show rolling UI
			UIManager.instance.ShowRollingUI();

			// show player UI.
			UIManager.instance.ShowPlayersUI();
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
		}

		void Square_initSquareEvent (Square square)
		{
			int index = square.PosIndex;
			GameObject squareObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Square"));
			squareObj.transform.SetParent(boardGameObj.transform);
			Sprite normalSquareLay = Resources.Load<Sprite>("Images/Board/normal_square_lay");
			Sprite cornerSquare = Resources.Load<Sprite>("Images/Board/corner_square");

			// start square, is GO square
			if (index == 0)
			{
				squareObj.GetComponent<SpriteRenderer>().sprite = cornerSquare;
				squareObj.transform.localPosition = new Vector3(0f, 0f, 0f);
			}

			// other squares
			else
			{
				GameObject prevSquare = squareGameObjs[index-1];
				float offsetX = 0f;
				float offsetY = 0f;
				if (index > 0 && index <= 10)
				{
					squareObj.GetComponent<SpriteRenderer>().sprite = index == 10 ? cornerSquare : normalSquareLay;
					offsetY = prevSquare.GetComponent<SpriteRenderer>().bounds.size.y;
				}
				else if (index >= 11 && index <= 20)
				{
					if (index == 20)
					{
						squareObj.GetComponent<SpriteRenderer>().sprite = cornerSquare;
					}
					offsetX = prevSquare.GetComponent<SpriteRenderer>().bounds.size.x;
				}
				else if (index >= 21 && index <= 30)
				{
					squareObj.GetComponent<SpriteRenderer>().sprite = index == 30 ? cornerSquare : normalSquareLay;
					offsetY = -squareObj.GetComponent<SpriteRenderer>().bounds.size.y;
				}
				else if (index >= 31 && index < 40)
				{
					offsetX = -squareObj.GetComponent<SpriteRenderer>().bounds.size.x;
				}

				squareObj.transform.localPosition = new Vector3(prevSquare.transform.localPosition.x + offsetX, 
				                                             prevSquare.transform.localPosition.y + offsetY, 
				                                             0f);
			}

			// add to square game object list
			//squareObj.GetComponent<SquareGameObject>().UpdateInfo(square);
			squareGameObjs.Add(squareObj);
		}

		void MovePlayer(int playerIndex, int srcIndex, int tgtIndex)
		{
			GameObject playerObj = playerGameObjs[playerIndex];
			int nextIndex = srcIndex + 1;
			// pass GO square
			if (nextIndex == LogicManager.SQUARE_COUNT)
			{
				LogicManager.instance.AddCashToPlayer(playerIndex, 200);
			}
			else if (nextIndex > LogicManager.SQUARE_COUNT)
			{
				nextIndex = nextIndex - LogicManager.SQUARE_COUNT;
			}
			//playerObj.transform.localPosition = GetCenterPositionOnSquare(nextIndex);
			Vector3 startPos = GetCenterPositionOnSquare(srcIndex);
			Vector3 endPos = GetCenterPositionOnSquare(tgtIndex);

			Debug.Log("start pos is " + startPos);
			Debug.Log("end pos is " + endPos);

			playerObj.GetComponent<PlayerGameObject>().Move(startPos, endPos);
			//yield return new WaitForSeconds(1f);
		}

		// after click dice
		void RollingUI_rollDiceEvent ()
		{
			// get numbers from logic manager.
			int[] nums = LogicManager.RollDice();

			// update UI
			UIManager.instance.UpdateDices(nums);

			// move player in logic data, will move game object in event callback.
			int delta = nums[0] + nums[1];
			LogicManager.instance.MovePlayer(delta);
		}

	}
}


