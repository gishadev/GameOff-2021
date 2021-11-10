using System;
using System.Collections.Generic;
using Gisha.GameOff_2021.Interactive;
using UnityEditor;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class ControllablesUIManager : MonoBehaviour
    {
        private static ControllablesUIManager Instance { get; set; }

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

                Vector2 screenPoint = HandleUtility.WorldToGUIPoint(controllables[i].transform.position);
                var element = Instantiate(Instance.controllableUIElementPrefab, Instance.parent);
                element.transform.position = screenPoint;

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

        public static Vector2 GetScreenPointFromWorld(Vector2 worldPoint)
        {
            var cam = Camera.main;
            float worldHeight = cam.orthographicSize;
            float worldWidth = Screen.width / Screen.height * cam.orthographicSize;

            float xPos = (worldPoint.x / worldWidth) * 100f;
            float yPos = (worldPoint.y / worldHeight) * 100f;

            return new Vector2(xPos, yPos);
        }
    }
}