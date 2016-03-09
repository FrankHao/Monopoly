using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

		public bool IsAutoMode {get; set;}
		GameObject boardGameObj;
		List<GameObject> squareGameObjs = new List<GameObject>();
		Dictionary<int, GameObject> playerGameObjs = new Dictionary<int, GameObject>();

		void Start() 
		{
			// UI event handlers
			EntryUI.gameStartEvent += EntryUI_gameStartEvent;	
			RollingUI.rollDiceEvent += RollingUI_rollDiceEvent;
			RollingUI.autoModeEvent += RollingUI_autoModeEvent;

			// logic event handlers
			Square.initSquareEvent += Square_initSquareEvent;
			Player.initPlayerEvent += Player_initPlayerEvent;
			Player.movedPlayerEvent += Player_movedPlayerEvent;
			Player.bankruptEvent += Player_bankruptEvent;
			LogicManager.changeTurnsEvent += LogicManager_changeTurnsEvent;
			LogicManager.gameStartEvent += LogicManager_gameStartEvent;
			LogicManager.gameOverEvent += LogicManager_gameOverEvent;

			// game object event handlers
			PlayerGameObject.passGoEvent += PlayerGameObject_passGoEvent;
			PlayerGameObject.movingEndEvent += PlayerGameObject_movingEndEvent;

		}

		// switch auto mode
		void RollingUI_autoModeEvent ()
		{
			instance.IsAutoMode = !instance.IsAutoMode;
		}

		void LogicManager_gameStartEvent ()
		{
			// when things are ready, show UI
			UIManager.instance.ShowRollingUI();
			UIManager.instance.ShowPlayersUI();
			//print("Game Start !");
		}


		void SaveResultToFile()
		{
			var file = File.CreateText(Application.persistentDataPath + "/result.txt");
			foreach(List<string> rd in LogicManager.instance.GetPlayHistory())
			{
				string newline = "";
				if (rd[0] == Constants.HISTORY_ROLL)
				{
					newline = string.Format("[{0}]: player {1} rolls {2} + {3}.", 
						rd[0], rd[1], rd[2], rd[3]);
				}
				else if (rd[0] == Constants.HISTORY_BUY)
				{
					newline = string.Format("[{0}]: player {1} buys {2}, cost {3}, left cash {4}.",
						rd[0], rd[1], rd[2], rd[3], rd[4]);
				}
				else if (rd[0] == Constants.HISTORY_RENT)
				{
					newline = string.Format("[{0}]: player {1} rent {2}, cost {3}, left cash {4}.",
						rd[0], rd[1], rd[2], rd[3], rd[4]);
				}
				file.WriteLine(newline);
			}

			foreach(Player player in LogicManager.instance.GetPlayers())
			{
				string resultline = "";
				resultline = string.Format("[RESULT]: player {0} cash {1}, owned properties {2}",
					player.PlayerIndex, player.Cash, MiniJSON.Json.Serialize(player.GetOwnedSquareIndex()));
				file.WriteLine(resultline);
			}

			file.Close();
		}

		void GameOverCallBack()
		{
			SaveResultToFile();
			RestartGame();
		}


		// TODO: restart game
		void RestartGame()
		{
			// release resource

			// reset logic data

			// restart game.
		}


		void LogicManager_gameOverEvent ()
		{
			instance.IsAutoMode = false;
			UIManager.instance.ShowPopupUI("Game Over, Result file saved to : " + 
				Application.persistentDataPath + "/result.txt", 
				GameOverCallBack);
			//print("Game Over !");
		}

		void Player_bankruptEvent (int playerIndex)
		{
			if (LogicManager.instance.ReadyToGameOver())
			{
				LogicManager.instance.GameOver();
			}
		}

		// release all events.
		void OnDestroy()
		{
			EntryUI.gameStartEvent -= EntryUI_gameStartEvent;
			RollingUI.rollDiceEvent -= RollingUI_rollDiceEvent;
			RollingUI.autoModeEvent -= RollingUI_autoModeEvent;
			Square.initSquareEvent -= Square_initSquareEvent;
			Player.initPlayerEvent -= Player_initPlayerEvent;
			Player.movedPlayerEvent -= Player_movedPlayerEvent;
			Player.bankruptEvent -= Player_bankruptEvent;
			LogicManager.changeTurnsEvent -= LogicManager_changeTurnsEvent;
			LogicManager.gameStartEvent -= LogicManager_gameStartEvent;
			LogicManager.gameOverEvent -= LogicManager_gameOverEvent;
			PlayerGameObject.passGoEvent -= PlayerGameObject_passGoEvent;
			PlayerGameObject.movingEndEvent -= PlayerGameObject_movingEndEvent;
		}

		void LogicManager_changeTurnsEvent (int playerIndex)
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

				// record buy history
				LogicManager.instance.RecordPlayHistory(Constants.HISTORY_BUY,
					playerIndex, squareIndex, value, cash);

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

		void PopupRentCallBack()
		{
			LogicManager.instance.ChangeTurns();
		}

		// moving end event handler
		void PlayerGameObject_movingEndEvent (int playerIndex, int squareIndex)
		{
			// game over can happen before player moving finish.
			if (LogicManager.instance.IsGameOver())
			{
				return;
			}

			Square sq = LogicManager.instance.GetSquare(squareIndex);
						
			// buyable square : property, station, utitlity
			if (sq.IsBuyable())
			{
				// hasn't been owned
				if (!sq.IsOwned())
				{
					if (instance.IsAutoMode)
					{
						ConfirmBuyCallBack();
						RollingUI_rollDiceEvent();
					}
					else
					{
						string title = string.Format("Do you want to buy this property ? Cost {0}.", sq.Value);
						UIManager.instance.ShowConfirmUI(title, ConfirmBuyCallBack, CancelBuyCallBack);	
					}
				}
				// owned by other players
				else if (sq.IsOwned() && sq.OwnerIndex != playerIndex)
				{
					// if not affordable, player bankrupts.
					if (!LogicManager.instance.IsAffordable(playerIndex, squareIndex))
					{
						LogicManager.instance.PlayerBankrupt(playerIndex);
						return;
					}
					
					long rentFee = LogicManager.instance.GetRentFee(playerIndex, squareIndex);
					string title = string.Format("You have to pay {0} for rent.", rentFee);

					// sub cash
					long cash = LogicManager.instance.SubCashFromPlayer(playerIndex, rentFee);

					// record rent history
					LogicManager.instance.RecordPlayHistory(Constants.HISTORY_RENT, 
						playerIndex, squareIndex, rentFee, cash);

					// update player UI
					UIManager.instance.UpdatePlayerCash(playerIndex, cash);

					if (instance.IsAutoMode)
					{
						PopupRentCallBack();
						RollingUI_rollDiceEvent();
					}
					else
					{
						// popup dialog UI
						UIManager.instance.ShowPopupUI(title, PopupRentCallBack);
					}
				}
				// owned by self, can do improvement.
				else if (sq.IsOwned() && sq.OwnerIndex == playerIndex)
				{
					LogicManager.instance.ChangeTurns();
					if (instance.IsAutoMode)
					{
						RollingUI_rollDiceEvent();
					}
				}
			}
			else
			{
				// change turns to another player.
				LogicManager.instance.ChangeTurns();
				if (instance.IsAutoMode)
				{
					RollingUI_rollDiceEvent();
				}
			}
		}

		// pass Go event handler
		void PlayerGameObject_passGoEvent (int playerIndex)
		{
			// add GO salary to player
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
			playerObj.transform.SetParent(instance.boardGameObj.transform);
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

		void GameStart() 
		{
			instance.IsAutoMode = false;

			// init board game object.
			InitBoard();

			// load board JSON file as TextAsset.
			TextAsset boardText = Resources.Load<TextAsset>("JSON/board");

			// init logic data
			// square game objects will be created in delegate event.
			LogicManager.instance.GameStart(boardText.text);
		}

		void InitBoard()
		{
			instance.boardGameObj = Instantiate(Resources.Load<GameObject>("prefabs/game/Board"));
			instance.boardGameObj.transform.SetParent(gameObject.transform);
		}

		void Square_initSquareEvent (Square square)
		{
			int index = square.SquareIndex;
			Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Board/squares");
			GameObject squareObj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Square"));
			squareObj.transform.SetParent(instance.boardGameObj.transform);

			// start square, is GO square
			if (index == 0)
			{
				//squareObj.GetComponent<SpriteRenderer>().sprite = cornerSquare;
				squareObj.GetComponent<SpriteRenderer>().sprite = sprites[index];
				float startX = boardGameObj.GetComponent<SpriteRenderer>().bounds.size.x - 
					squareObj.GetComponent<SpriteRenderer>().bounds.size.x;
				squareObj.transform.localPosition = new Vector3(startX, 0f, 0f);
			}

			// other squares
			else
			{
				float offsetX = 0f;
				float offsetY = 0f;
				GameObject prevSquare = instance.squareGameObjs[index-1];
				squareObj.GetComponent<SpriteRenderer>().sprite = sprites[index];
				if (index > 0 && index <= Constants.SQUARE_COUNT_EACH_SIDE)
				{
					offsetX = -squareObj.GetComponent<SpriteRenderer>().bounds.size.x;
				}
				else if (index >= 11 && index <= 20)
				{
					offsetY = prevSquare.GetComponent<SpriteRenderer>().bounds.size.y;
				}
				else if (index >= 21 && index <= 30)
				{
					offsetX = prevSquare.GetComponent<SpriteRenderer>().bounds.size.x;
				}
				else if (index >= 31 && index < Constants.TOTAL_SQUARE_COUNT)
				{
					offsetY = -squareObj.GetComponent<SpriteRenderer>().bounds.size.y;
				}

				squareObj.transform.localPosition = new Vector3(prevSquare.transform.localPosition.x + offsetX, 
				                                             prevSquare.transform.localPosition.y + offsetY, 
				                                             0f);
			}

			// add to square game object list
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
			LogicManager.instance.MovePlayer(nums);
		}

	}
}


