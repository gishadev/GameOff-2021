using UnityEngine;

namespace Gisha.GameOff_2021.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Header("General")] [SerializeField] private Transform spawnpoint;
        [Header("Bounds")] [SerializeField] private Transform leftBound;
        [SerializeField] private Transform rightBound;

        public Transform RightBound => rightBound;
        public Transform LeftBound => leftBound;


        [ContextMenu("Try Get Bounds")]
        private void TryGetBounds()
        {
            rightBound = transform.Find("Right");
            leftBound = transform.Find("Left");
        }
    }
}