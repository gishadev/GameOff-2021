using System;
using TMPro;
using UnityEngine;

namespace Gisha.GameOff_2021.Utilities
{
    public class Timer : MonoBehaviour
    {
        private static Timer Instance { get;  set; }
        
        [SerializeField] private TMP_Text timerText;

        private float _startTime;
        
        private void Awake()
        {
            Instance = this;
            _startTime = Time.time;
        }

        private void Update()
        {
            float ms = (Time.time - _startTime) * 1000;
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            timerText.text = $"{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds:000}";
        }

        public static void Restart()
        {
            Instance._startTime = Time.time;
        }
    }
}