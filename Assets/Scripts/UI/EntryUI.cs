using UnityEngine;
using System.Collections;
using DigitalRuby.Tween;


namespace Monopoly.View
{
    public class EntryUI : MonoBehaviour
    {

        public delegate void gameStart();
        public static event gameStart gameStartEvent;

        [SerializeField] GameObject playButton;

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
            TweenRotate();
            TweenSize();
        }

        private void TweenRotate()
        {
            System.Action<ITween<float>> circleRotate = (t) =>
            {
                // start rotation from identity to ensure no stuttering
                transform.rotation = Quaternion.identity;
                transform.Rotate(Camera.main.transform.forward, t.CurrentValue);
            };

            float startAngle = transform.rotation.eulerAngles.z;
            float endAngle = startAngle + 720.0f + 45f;

            // completion defaults to null if not passed in
            gameObject.Tween("RotateCircle", startAngle, endAngle, 1.0f, TweenScaleFunctions.CubicEaseInOut, circleRotate);
        }
        private void TweenSize()
        {
            System.Action<ITween<float>> circleSize = (t) =>
            {
                // start rotation from identity to ensure no stuttering
                transform.rotation = Quaternion.identity;
                transform.localScale = Vector3.one * t.CurrentValue;
            };

            float startAngle = 1;
            float endAngle = 0.75f;

            // completion defaults to null if not passed in
            gameObject.Tween("ResizeCircle", startAngle, endAngle, 1.0f, TweenScaleFunctions.CubicEaseInOut, circleSize);
        }

    }
}


