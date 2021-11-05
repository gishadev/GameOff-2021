using System;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 500f;
        [SerializeField] private float jumpForce = 8f;

        [Header("Ground Checker")] [Space] [SerializeField]
        private Transform groundCheckerPoint;

        [SerializeField] private float groundCheckerRadius = 0.25f;

        private LayerMask _groundLayer;
        private bool _isGrounded = true;
        private float _hInput;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        }

        private void Update()
        {
            HandleInput();
            CheckForGround();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_hInput * moveSpeed * Time.deltaTime, _rb.velocity.y);
        }

        private void HandleInput()
        {
            _hInput = Input.GetAxis("Horizontal");

            // Making full jump.
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

            // Making half jump.
            if (Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0)
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        private void CheckForGround()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheckerPoint.position, groundCheckerRadius, _groundLayer);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckerPoint.position, groundCheckerRadius);
        }
    }
}