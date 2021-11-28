using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class InvisibleBound : MonoBehaviour, IEnterBound
    {
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            GameManager.LevelChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
            GameManager.LevelChanged -= OnLevelChanged;
        }

        public void OnLevelChanged(LevelManager levelManager)
        {
            // Auto-closing of the bound, when the player enters a new level.
            if (gameObject.scene == levelManager.LevelScene)
                _collider.enabled = true;
        }
    }
}