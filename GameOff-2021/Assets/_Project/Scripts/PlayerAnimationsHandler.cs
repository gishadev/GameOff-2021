using System;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class PlayerAnimationsHandler : MonoBehaviour
    {
        private Animator _animator;
        private PlayerController _controller;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _controller.OnPlayerJumped += SetJumpAnimation;
        }

        private void OnDisable()
        {
            _controller.OnPlayerJumped -= SetJumpAnimation;
        }

        private void Update()
        {
            if (_controller.Velocity.magnitude > 0)
                _animator.SetBool("IsRunning", true);
            else
                _animator.SetBool("IsRunning", false);
        }

        private void SetJumpAnimation()
        {
            _animator.SetTrigger("Jump");
        }
    }
}