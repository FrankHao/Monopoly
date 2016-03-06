using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Monopoly.View;
using Monopoly.Model;

namespace Monopoly.Controller
{
	public class GameManager : MonoBehaviour {

		GameObject gameBoard;

		

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
			
		}

		// release all events, resources, reference.
		void OnDestroy()
		{
			EntryUI.gameStartEvent -= EntryUI_gameStartEvent;
			RollingUI.rollDiceEvent -= RollingUI_rollDiceEvent;
			Square.initSquareEvent -= Square_initSquareEvent;

			Destroy(gameBoard);
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
			gameBoard = Instantiate(Resources.Load<GameObject>("prefabs/game/Board"));
			gameBoard.transform.SetParent(gameObject.transform);
		}


		// use square data to init game object.
		void Square_initSquareEvent (Square squareObj, int index)
		{
			GameObject square = Instantiate(Resources.Load<GameObject>("Prefabs/Game/Square"));
			square.transform.SetParent(gameBoard.transform);
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


		}

	}
}


