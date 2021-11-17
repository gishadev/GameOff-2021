using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class LevelBounds : MonoBehaviour
    {
        public static LevelBounds Instance { get; private set; }
        
        [Header("Bounds")] [SerializeField] private Transform leftBound;
        [SerializeField] private Transform rightBound;

        public static Transform RightBound => Instance.rightBound;
        public static Transform LeftBound => Instance.leftBound;

        private void Awake()
        {
            Instance = this;
        }

        [ContextMenu("Try Get Bounds")]
        private void TryGetBounds()
        {
            rightBound = transform.Find("Right");
            leftBound = transform.Find("Left");
        }
    }
}