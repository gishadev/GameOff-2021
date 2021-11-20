using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Door : Controllable, IUITriggerControl
    {
        /// <summary>
        /// If the door is entry it closes permanently, when the players enters a new level. 
        /// </summary>
        [SerializeField] private bool _isEnterDoor = false;

        private Animator _animator;
        private Collider2D _collider;

        private bool _isOpened = false;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameManager.LevelChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
            GameManager.LevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(LevelManager levelManager)
        {
            // Auto-closing of the door, when the player enters a new level.
            if (_isEnterDoor && gameObject.scene == levelManager.LevelScene)
            {
                GameManager.ControllableList.Remove(this);
                _isOpened = false;
                _animator.SetBool("IsOpened", _isOpened);
                _collider.enabled = true;
            }
        }

        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_TriggerBtn);
        }

        public void OnClick_TriggerBtn()
        {
            _isOpened = !_isOpened;
            _animator.SetBool("IsOpened", _isOpened);
            _collider.enabled = !_isOpened;
        }
    }
}