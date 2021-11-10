using System;
using System.Collections.Generic;
using Gisha.GameOff_2021.Interactive;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class ControllablesUIManager : MonoBehaviour
    {
        private static ControllablesUIManager Instance { get;  set; }
        
        [SerializeField] private Transform parent;
        [SerializeField] private GameObject controllableUIElementPrefab;

        private List<GameObject> controllableUIElementsList = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public static void SpawnControllableUIElements(Controllable[] controllables)
        {
            for (int i = 0; i < controllables.Length; i++)
            {
                if (controllables[i] == null)
                    continue;

                Vector2 screenPoint = Camera.main.WorldToScreenPoint(controllables[i].transform.position);
                var element = Instantiate(Instance.controllableUIElementPrefab, screenPoint, Quaternion.identity,
                    Instance.parent);
                Instance.controllableUIElementsList.Add(element);
            }
        }

        public static void RemoveControllableUIElements()
        {
            var list = Instance.controllableUIElementsList;
            
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Destroy(list[i]);
                list.RemoveAt(i);
            }
        }
    }
}