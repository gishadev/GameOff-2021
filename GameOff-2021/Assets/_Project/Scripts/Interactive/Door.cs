using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Door : Controllable, IUITriggerControl
    {
        private Animator _animator;
        private Collider2D _collider;

        private bool _isOpened = false;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
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