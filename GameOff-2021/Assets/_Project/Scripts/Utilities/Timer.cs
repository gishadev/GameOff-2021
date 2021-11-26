using System;
using TMPro;
using UnityEngine;

namespace Gisha.GameOff_2021.Utilities
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        private void Update()
        {
            float ms = Time.time * 1000;
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            timerText.text = $"{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds:000}";
        }
    }
}