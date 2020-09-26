using UnityEngine;
using System.Collections;
using DigitalRuby.Tween;


namespace Monopoly.View
{
    public class EntryUI : MonoBehaviour
    {

        public delegate void gameStart();
        public static event gameStart gameStartEvent;

        [SerializeField] GameObject playButton = null;

        public void OnClickPlayBtn()
        {
            playButton.gameObject.SetActive(false);
            EntryShow();

            if (gameStartEvent != null)
            {
                gameStartEvent();
            }
        }

        void EntryShow()
        {
            TweenUtil.TweenRotate(transform, transform.rotation.eulerAngles.z + 720f + 45f);
            TweenUtil.TweenSize(transform, 0.75f);
        }

    }
}


