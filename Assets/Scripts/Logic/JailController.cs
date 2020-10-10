using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monopoly.Common;
using Monopoly.View;
using Monopoly.Model;

namespace Monopoly.Controller
{
    public class JailController
    {
        public void QuestionPlayer(int playerIndex)
        {
            Player player = LogicManager.instance.GetPlayer(playerIndex);

            Debug.Log($"Player jail time = {player.JailTime}");
            UIManager.instance.ShowJailQuestionUI((questionUI) =>
            {
                System.Action onComplete = delegate
                {
                    Object.Destroy(questionUI.gameObject);
                    UIManager.instance.IsShowingDialogUI = false;
                };

                if (player.JailBailCards > 0)
                {
                    questionUI.OnUseCard = () =>
                    {
                        onComplete();
                        player.UseJailCard(callbackOnOK: () =>
                        {
                            player.BailOutJail(0);
                            UIManager.instance.EnableDice(true);
                        }, callBackOnCancel: () =>
                        {
                            this.QuestionPlayer(player.PlayerIndex);
                        });

                    };
                }

                if (player.JailTime > 0)
                {
                    // can stay in jail
                    questionUI.OnStay = () =>
                    {
                        player.ServeJailTime();
                        onComplete();
                        LogicManager.instance.ChangeTurns();
                    };

                    questionUI.OnRollDice = () =>
                    {
                        onComplete();
                        JailRollDice();
                    };
                }

                // Pay to get out during 1st and 2nd turn
                if (player.JailTime > 1)
                {
                    questionUI.OnPayBail = () =>
                    {
                        onComplete();
                        bool bailOk = player.BailOutJail();
                        if (bailOk)
                        {
                            UIManager.instance.EnableDice(true);
                        }
                    };
                }
            });
        }

        void JailRollDice()
        {
            UIManager.instance.ShowPopupUI("Roll a double to get out from Jail free.", () =>
            {
                UIManager.instance.EnableDice(true);
            });
        }

    }
}
