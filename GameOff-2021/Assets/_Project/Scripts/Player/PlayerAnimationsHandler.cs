using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    public class PlayerAnimationsHandler : MonoBehaviour
    {
        private float _hInput;

        private Animator _animator;
        private MovementBehaviour _movementBehaviour;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
            _movementBehaviour = GetComponent<MovementBehaviour>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _movementBehaviour.PlayerJumped += SetJumpAnimation;
        }

        private void OnDisable()
        {
            _movementBehaviour.PlayerJumped -= SetJumpAnimation;
        }

        private void Update()
        {
            _hInput = Input.GetAxisRaw("Horizontal");
            
            HandleAnimations();
            HandleSpriteFlip();
        }

        private void HandleAnimations()
        {
            if (Mathf.Abs(_hInput) > 0 && _movementBehaviour.enabled)
                _animator.SetBool("IsRunning", true);
            else
                _animator.SetBool("IsRunning", false);
        }

        private void HandleSpriteFlip()
        {
            if (_hInput > 0)
                _sr.flipX = false;
            else if (_hInput < 0)
                _sr.flipX = true;
        }


        private void SetJumpAnimation()
        {
            _animator.SetTrigger("Jump");
        }
    }
}