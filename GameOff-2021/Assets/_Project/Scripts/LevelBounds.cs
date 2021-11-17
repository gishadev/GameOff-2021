using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class LevelBounds : MonoBehaviour
    {
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