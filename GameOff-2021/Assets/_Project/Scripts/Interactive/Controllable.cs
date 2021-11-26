using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gisha.GameOff_2021.Interactive
{
    public abstract class Controllable : MonoBehaviour
    {
        [SerializeField] private GameObject visualElementPrefab;

        public GameObject VisualElementPrefab => visualElementPrefab;
        public List<UnityAction> InteractActions
        {
            get => _interactActions;
            set => _interactActions = value;
        }
        
        [HideInInspector]
        public GameObject visualElement;
        
        private List<UnityAction> _interactActions = new List<UnityAction>();
        
        protected abstract void OnAddInteractActions();

        private void Start()
        {
            OnAddInteractActions();
        }

        private void OnDestroy()
        {
            ControllablesVisualizer.RemoveControllableUIElement(visualElement);
            Destroy(visualElement);
        }
    }
}