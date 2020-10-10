using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Monopoly.Model;
using Monopoly.Controller;

namespace Monopoly.View
{
    public class UIManager : MonoBehaviour
    {
        public List<Color> OwnerColor;

        #region singleton
        // singleton design partten
        private UIManager() { }
        private static UIManager _instance;
        public static UIManager instance
        {
            get
            {
                // potentially if we have multi scenes
                // need to take care of duplicated issue, when switch between different scenes.
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        #endregion

        GameObject entryUI;
        GameObject rollingUI;
        GameObject playersUI;
        GameObject confirmUI;
        GameObject popupUI;
        GameObject squareShowingObject;
        public bool IsShowingDialogUI { get; set; }

        void Awake()
        {
            InitUI();
        }

        void InitUI()
        {
            instance.entryUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/EntryUI"));
            instance.entryUI.transform.SetParent(gameObject.transform, false);

            instance.rollingUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/RollingUI"));
            instance.rollingUI.transform.SetParent(gameObject.transform, false);
            instance.rollingUI.SetActive(false);

            instance.playersUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayersUI"));
            instance.playersUI.transform.SetParent(gameObject.transform, false);
            instance.playersUI.SetActive(false);

            instance.confirmUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConfirmUI"));
            instance.confirmUI.transform.SetParent(gameObject.transform, false);
            instance.confirmUI.SetActive(false);

            instance.popupUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PopupUI"));
            instance.popupUI.transform.SetParent(gameObject.transform, false);
            instance.popupUI.SetActive(false);

        }

        public void UpdateDices(int[] nums)
        {
            instance.rollingUI.GetComponent<RollingUI>().UpdateDices(nums[0], nums[1]);
        }

        public void ShowRollingUI()
        {
            instance.rollingUI.SetActive(true);
        }

        public void ShowPlayersUI()
        {
            instance.playersUI.SetActive(true);
        }

        public void ShowPopupUI(string title, PopupUI.GenericCallBack callback)
        {
            instance.popupUI.GetComponent<PopupUI>().UpdateInfo(title, callback);
            instance.popupUI.SetActive(true);
        }

        public void ShowJailQuestionUI(System.Action<JailQuestionUI> callback)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/JailQuestionUI"));
            go.transform.SetParent(gameObject.transform, false);
            callback(go.GetComponent<JailQuestionUI>());
            IsShowingDialogUI = true;
        }

        public void ShowSquareDescription(int squareNumber)
        {
            if (instance.squareShowingObject != null || instance.IsShowingDialogUI) { return; }

            Square square = LogicManager.instance.GetSquare(squareNumber);
            switch (square.Type)
            {
                case "property":
                    instance.squareShowingObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/PropertyCardUI"));
                    PropertyCardUI card = instance.squareShowingObject.GetComponent<PropertyCardUI>();
                    instance.squareShowingObject.transform.SetParent(gameObject.transform, false);
                    card.SetCard(square);
                    break;
                case "chance":
                case "chest":
                case "tax":
                    return;
                case "go":
                case "free":
                    return;
                case "gotojail":
                case "jail":
                    return;
                case "station":
                    instance.squareShowingObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/RailroadCardUI"));
                    RailroadCardUI rc = instance.squareShowingObject.GetComponent<RailroadCardUI>();
                    instance.squareShowingObject.transform.SetParent(gameObject.transform, false);
                    rc.SetCard(square);
                    break;

                default:
                    instance.squareShowingObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GenericCardUI"));
                    GenericCardUI gc = instance.squareShowingObject.GetComponent<GenericCardUI>();
                    instance.squareShowingObject.transform.SetParent(gameObject.transform, false);
                    gc.SetCard(square);
                    break;
            }

            instance.squareShowingObject.SetActive(true);
        }

        public void ShowConfirmUI(string title,
            System.Action okCallBack,
            System.Action cancelCallBack)
        {
            instance.confirmUI.SetActive(true);
            instance.confirmUI.GetComponent<ConfirmUI>().UpdateInfo(title, okCallBack, cancelCallBack);
        }

        public void AddPlayerInfo(int playerIndex, string name, long cash)
        {
            instance.playersUI.GetComponent<PlayersUI>().AddPlayerPanel(playerIndex, name, cash);
        }

        public void UpdatePlayerCash(int playerIndex, long cash)
        {
            instance.playersUI.GetComponent<PlayersUI>().UpdatePlayerCash(playerIndex, cash);
        }

        public void UpdateCurrentPlayerIndex(int playerIndex, bool canRoll)
        {
            instance.playersUI.GetComponent<PlayersUI>().UpdateCurrentPlayerIndex(playerIndex);
            EnableDice(canRoll);
        }

        public void EnableDice(bool canRoll)
        {
            instance.rollingUI.GetComponent<RollingUI>().EnableRollButton(canRoll);
        }
    }

}
