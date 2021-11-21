using System;
using UnityEngine;

namespace Gisha.GameOff_2021.GUI
{
    public class HealthGUI : MonoBehaviour
    {
        private static HealthGUI Instance { get; set; }

        private GameObject[] _hpUIElements;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _hpUIElements = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
                _hpUIElements[i] = transform.GetChild(i).gameObject;
        }

        public static void ChangeHPCount(int count)
        {
            count--;
            
            for (int i = 0; i < Instance._hpUIElements.Length; i++)
            {
                if (i < count)
                    Instance._hpUIElements[i].SetActive(true);
                else
                    Instance._hpUIElements[i].SetActive(false);
            }
        }
    }
}