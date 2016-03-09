using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Monopoly.View;
using Monopoly.Model;
using Monopoly.Common;

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
					DontDestroyOnLoad(_instance.gameObject);
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
			// UI event handlers
			EntryUI.gameStartEvent += EntryUI_gameStartEvent;	
			RollingUI.rollDiceEvent += RollingUI_rollDiceEvent;

			// logic event handlers
			Square.initSquareEvent += Square_initSquareEvent;
			Player.initPlayerEvent += Player_initPlayerEvent;
			Player.movedPlayerEvent += Player_movedPlayerEvent;
			LogicManager.setPlayerIndexEvent += LogicManager_setPlayerIndexEvent;

			// game object event handlers
			PlayerGameObject.passGoEvent += PlayerGameObject_passGoEvent;
			PlayerGameObject.movingEndEvent += PlayerGameObject_movingEndEvent;

		}

		// release all events.
		void OnDestroy()
		{
			EntryUI.gameStartEvent -= EntryUI_gameStartEvent;
			RollingUI.rollDiceEvent -= RollingUI_rollDiceEvent;
			Square.initSquareEvent -= Square_initSquareEvent;
			Player.initPlayerEvent -= Player_initPlayerEvent;
			Player.movedPlayerEvent -= Player_movedPlayerEvent;
			LogicManager.setPlayerIndexEvent -= LogicManager_setPlayerIndexEvent;
			PlayerGameObject.passGoEvent -= PlayerGameObject_passGoEvent;
			PlayerGameObject.movingEndEvent -= PlayerGameObject_movingEndEvent;
		}

		void LogicManager_setPlayerIndexEvent (int playerIndex)
		{
			// update current player index
			UIManager.instance.UpdateCurrentPlayerIndex(playerIndex);
		}

		void ConfirmBuyCallBack()
		{
			int playerIndex = LogicManager.instance.CurrentPlayerIndex;
			int squareIndex = LogicManager.instance.GetPlayerSquareIndex(playerIndex);
			long value = LogicManager.instance.GetSquareValue(squareIndex);
			long playerCash = LogicManager.instance.GetPlayerCash(playerIndex);
			if (value <= playerCash)
			{
				// sub cash
				long cash = LogicManager.instance.SubCashFromPlayer(playerIndex, value);

				// update logic data
				LogicManager.instance.PlayerOwnSquare(playerIndex, squareIndex);

				// update UI
				UIManager.instance.UpdatePlayerCash(playerIndex, cash);
			}

			LogicManager.instance.ChangeTurns();
		}

		void CancelBuyCallBack()
		{
			//TODO: can trigger auction event here.

			// change turn to another player.
			LogicManager.instance.ChangeTurns();
		}

		// moving end event handler
		void PlayerGameObject_movingEndEvent (int playerIndex, int squareIndex)
		{
			Square sq = LogicManager.instance.GetSquare(squareIndex);
			// property square
			if (sq.Type == Constants.SQ_PROPERTY && sq.IsOwned() == false)
			{
				string title = string.Format("Do you want to buy this property ? Cost {0}.", sq.Value);
				UIManager.instance.ShowConfirmUI(title, ConfirmBuyCallBack, CancelBuyCallBack);	
			}
			else
			{
				// change turns to another player.
				LogicManager.instance.ChangeTurns();
			}
		}

		// pass Go event handler
		void PlayerGameObject_passGoEvent (int playerIndex)
		{
			// add salary to player
			long cash = LogicManager.instance.AddCashToPlayer(playerIndex, (long)Constants.GO_PASS_SALARY);

			// update cash
			UIManager.instance.UpdatePlayerCash(playerIndex, cash);
		}



		// moved player event handler
		void Player_movedPlayerEvent(int playerIndex, List<int> pathList)
		{
			GameObject playerObj = instance.playerGameObjs[playerIndex];

			// transfer square index list to position vector3 queue
			Queue<KeyValuePair<int, Vector3>> pathQueue = new Queue<KeyValuePair<int, Vector3>>();
			foreach(int sqIndex in pathList)
			{
				Vector3 pos = GetCenterPositionOnSquare(sqIndex);
				pathQueue.Enqueue(new KeyValuePair<int, Vector3>(sqIndex, pos));
			}

			// move player game object
			playerObj.GetComponent<PlayerGameObject>().Move(pathQueue);
		}

		// get the center position on specific square, as square pivot is bottom left.
		Vector3 GetCenterPositionOnSquare(int squareIndex)
		{
			GameObject squareObj = instance.squareGameObjs[squareIndex];
			Vector3 pos = squareObj.transform.localPosition;
			Vector3 extents = squareObj.GetComponent<SpriteRenderer>().bounds.extents;
			return new Vector3(pos.x + extents.x, pos.y + extents.y, 0f);
		}

		void Player_initPlayerEvent (Player player)
		{
			// create player game object
			GameObject playerObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Player"));
			playerObj.transform.SetParent(boardGameObj.transform);
			playerObj.GetComponent<PlayerGameObject>().PlayerIndex = player.PlayerIndex;
			playerObj.transform.localPosition = GetCenterPositionOnSquare(Constants.GO_SQAURE_INDEX);
			instance.playerGameObjs.Add(player.PlayerIndex, playerObj);

			// create player UI object
			UIManager.instance.AddPlayerInfo(player.PlayerIndex, player.Name, player.Cash);
		}
			
		void EntryUI_gameStartEvent ()
		{
			if (LogicManager.instance.State == Constants.GAME_NOT_START)
			{
				GameStart();
			}
		}

		void StartGameCallBack()
		{
			// when things are ready, show UI
			UIManager.instance.ShowRollingUI();
			UIManager.instance.ShowPlayersUI();
		}

		void GameStart() 
		{
			// init board game object.
			InitBoard();

			// load board JSON file as TextAsset.
			TextAsset boardText = Resources.Load<TextAsset>("JSON/board");

			// init logic data
			// square game objects will be created in delegate event.
			LogicManager.instance.StartGame(boardText.text, StartGameCallBack);
		}

		void InitBoard()
		{
			instance.boardGameObj = Instantiate(Resources.Load<GameObject>("prefabs/game/Board"));
			instance.boardGameObj.transform.SetParent(gameObject.transform);
		}

		void Square_initSquareEvent (Square square)
		{
			int index = square.SquareIndex;
			GameObject squareObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Square"));
			squareObj.transform.SetParent(instance.boardGameObj.transform);
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
				GameObject prevSquare = instance.squareGameObjs[index-1];
				float offsetX = 0f;
				float offsetY = 0f;
				if (index > 0 && index <= Constants.SQUARE_COUNT_EACH_SIDE)
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
				else if (index >= 31 && index < Constants.TOTAL_SQUARE_COUNT)
				{
					offsetX = -squareObj.GetComponent<SpriteRenderer>().bounds.size.x;
				}

				squareObj.transform.localPosition = new Vector3(prevSquare.transform.localPosition.x + offsetX, 
				                                             prevSquare.transform.localPosition.y + offsetY, 
				                                             0f);
			}

			// add to square game object list
			//squareObj.GetComponent<SquareGameObject>().UpdateInfo(square);
			instance.squareGameObjs.Add(squareObj);
		}

		// after click dice
		void RollingUI_rollDiceEvent ()
		{
			// get two dice numbers.
			int[] nums = Utilities.GetTwoDiceNumbers();

			// update UI
			UIManager.instance.UpdateDices(nums);

			// move player in logic data, will move game object in event callback.
			int delta = nums[0] + nums[1];
			LogicManager.instance.MovePlayer(delta);
		}

		public void EnterScene2()
		{
			SceneManager.LoadScene("test");
		}

	}
}


