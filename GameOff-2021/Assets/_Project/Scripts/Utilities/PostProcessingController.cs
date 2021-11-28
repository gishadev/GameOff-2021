using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Gisha.GameOff_2021.Utilities
{
    public class PostProcessingController : MonoBehaviour
    {
        public static PostProcessingController Instance { get; private set; }

        [SerializeField] private float defaultVignette;
        [SerializeField] private float controlVignette;
        [SerializeField] private float changingSmoothness;

        private Volume _ppVolume;
        private float _currentVignette, _newVignette;

        private void Awake()
        {
            Instance = this;

            _ppVolume = GetComponent<Volume>();

            SetDefaultPreset();
        }

        private void Update()
        {
            if (Math.Abs(_currentVignette - _newVignette) > 0.05f)
            {
                _currentVignette = Mathf.Lerp(_currentVignette, _newVignette, Time.deltaTime * changingSmoothness);
                SetVignette(_currentVignette);
            }
        }

        public static void SetControlPreset()
        {
            Instance._newVignette = Instance.controlVignette;
        }

        public static void SetDefaultPreset()
        {
            Instance._newVignette = Instance.defaultVignette;
        }

        private void SetVignette(float value)
        {
            Instance._ppVolume.profile.TryGet(out Vignette vignette);

            if (vignette != null)
                vignette.intensity.value = value;
        }
    }
}