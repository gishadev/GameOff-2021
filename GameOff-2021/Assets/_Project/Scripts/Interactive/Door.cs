using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Door : Controllable
    {
        private Animator _animator;
        private Collider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        public override void InteractAction()
        {
            _animator.SetTrigger("Open");
            _collider.enabled = false;
        }
    }
}