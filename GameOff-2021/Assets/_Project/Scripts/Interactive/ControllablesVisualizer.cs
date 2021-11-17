using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gisha.GameOff_2021.Interactive
{
    public class ControllablesVisualizer : MonoBehaviour
    {
        private static ControllablesVisualizer Instance { get; set; }

        private List<GameObject> controllableVisualsList = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public static void SpawnControllableVisuals(Controllable[] controllables)
        {
            for (int i = 0; i < controllables.Length; i++)
            {
                if (controllables[i] == null)
                    continue;

                // Instantiate visual object.
                var visualObject = Instantiate(controllables[i].VisualElementPrefab, Instance.transform);
                Instance.StartCoroutine(UpdateVisualPositionCoroutine(controllables[i], visualObject.transform));

                // Initialize visual element and connect it to certain controllable.
                var buttons = visualObject.GetComponentsInChildren<Button>();
                var visualElement = new ControllableVisualElement(controllables[i], buttons);
                visualElement.ConnectUIToControllable();

                Instance.controllableVisualsList.Add(visualObject);
            }
        }

        public static void RemoveControllableUIElements()
        {
            var list = Instance.controllableVisualsList;
            Instance.StopAllCoroutines();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                Destroy(list[i]);
                list.RemoveAt(i);
            }
        }

        private static IEnumerator UpdateVisualPositionCoroutine(Controllable controllable, Transform visual)
        {
            while (true)
            {
                visual.position = controllable.transform.position;
                yield return null;
            }
        }
    }

    public class ControllableVisualElement
    {
        private Button[] _buttons;
        private Controllable _controllable;

        public ControllableVisualElement(Controllable controllable, Button[] buttons)
        {
            _controllable = controllable;
            _buttons = buttons;
        }

        public void ConnectUIToControllable()
        {
            for (var i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];
                button.onClick.AddListener(_controllable.InteractActions[i]);
            }
        }
    }
}