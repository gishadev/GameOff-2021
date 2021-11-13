using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public abstract class Controllable : MonoBehaviour
    {
        [SerializeField] private GameObject visualElementPrefab;
        
        public GameObject VisualElementPrefab => visualElementPrefab;

        public abstract void InteractAction();
    }
}