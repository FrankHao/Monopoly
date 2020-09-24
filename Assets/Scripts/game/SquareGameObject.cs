using UnityEngine;
using Monopoly.View;
using Monopoly.Common;

namespace Monopoly.Controller
{
    public class SquareGameObject : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer OwnerDot;

        static readonly Vector3[] LocalPositions = new Vector3[] {
            new Vector3(.65f, 2f, 0), // UP
            new Vector3(2f, .65f, 0), // Right
            new Vector3(.65f, 0, 0), // DOWN
            new Vector3(0, .65f, 0)   // Left
	    };

        private void Awake()
        {
            // set ownerdot to transparent 
            AssignColor(new Color(0, 0, 0, 0));
        }

        public void AssignOwner(int playerIdx)
        {
            AssignColor(UIManager.instance.OwnerColor[playerIdx]);
        }

        public void AdjustLocation(Constants.Direction direction)
        {
            Vector3 pos = Vector3.zero;
            switch (direction)
            {
                case Constants.Direction.UP:
                    pos = LocalPositions[0];
                    break;
                case Constants.Direction.RIGHT:
                    pos = LocalPositions[1];
                    break;
                case Constants.Direction.DOWN:
                    pos = LocalPositions[2];
                    break;
                case Constants.Direction.LEFT:
                    pos = LocalPositions[3];
                    break;
            }
            OwnerDot.transform.localPosition = pos;
        }

        void AssignColor(Color color)
        {
            OwnerDot.color = color;
        }
    }
}


