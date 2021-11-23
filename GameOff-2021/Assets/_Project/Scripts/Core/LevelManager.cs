using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Header("General")] [SerializeField] private Transform spawnpoint;
        [SerializeField] private bool isVerticalExit = false;
        [SerializeField] private bool isReversedExit = false;
        [Header("Bounds")] [SerializeField] private Transform leftBound;
        [SerializeField] private Transform rightBound;

        public Transform RightBound => rightBound;
        public Transform LeftBound => leftBound;
        public Transform Spawnpoint => spawnpoint;
        public bool IsVerticalExit => isVerticalExit;
        public bool IsReversedExit => isReversedExit;
        public Scene LevelScene => gameObject.scene;


        [ContextMenu("Try Get Bounds")]
        private void TryGetBounds()
        {
            rightBound = transform.Find("Right");
            leftBound = transform.Find("Left");
        }
    }
}