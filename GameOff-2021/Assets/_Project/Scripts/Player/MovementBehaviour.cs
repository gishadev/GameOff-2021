using UnityEngine;
using System;
using System.Collections;
using Gisha.GameOff_2021.Core;
using Gisha.GameOff_2021.Interactive;

namespace Gisha.GameOff_2021.Player
{
    internal class MovementBehaviour : PlayerBehaviour
    {
        [SerializeField] private float moveSpeed = 500f;
        [SerializeField] private float jumpForce = 23f;
        [SerializeField] private float coyoteTime = 0.15f;
        [SerializeField] private float jumpBufferTime = 0.2f;

        [Header("Ground Checker")] [SerializeField]
        private Transform groundCheckerPoint;
        [SerializeField] private float groundCheckerRadius = 0.31f;
        [SerializeField] private float afterJumpCheckDelay = 0.1f;

        [Space]
        [SerializeField] private PhysicsMaterial2D maxFrictionMaterial;
        
        public Vector2 Velocity => _rb.velocity;
        public Action OnPlayerJumped { set; get; }
        
        private float _coyoteCounter;
        private float _bufferCounter;
        private LayerMask _groundLayer;
        private bool _isGrounded = true;
        private float _hInput;

        private Rigidbody2D _rb;
        private Collider2D _coll;
        
        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        }

        private void Start()
        {
            StartCoroutine(GroundCheckCoroutine());
        }

        public override void Update()
        {
            HandleInput();
        }

        public override void FixedUpdate()
        {
            _rb.velocity = new Vector2(_hInput * moveSpeed * Time.deltaTime, _rb.velocity.y);
        }

        public override void ResetOnBehaviourChange()
        {
            _coll.sharedMaterial = maxFrictionMaterial;
            
            ControllablesVisualizer.SpawnControllableVisuals(GameManager.ControllableList);
        }

        private void HandleInput()
        {
            _hInput = Input.GetAxis("Horizontal");

            // Coyote counter.
            if (_isGrounded)
                _coyoteCounter = coyoteTime;
            else
                _coyoteCounter -= Time.deltaTime;

            // Jump buffer counter.
            if (Input.GetKeyDown(KeyCode.Space))
                _bufferCounter = jumpBufferTime;
            else
                _bufferCounter -= Time.deltaTime;

            // Make Jump.
            if (_bufferCounter > 0 && _coyoteCounter > 0)
                Jump();

            // Shorten jump height.
            if (Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0)
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        private void Jump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

            _isGrounded = false;
            _coyoteCounter = 0;
            _bufferCounter = 0;

            StopAllCoroutines();
            StartCoroutine(GroundCheckCoroutineWithDelay(afterJumpCheckDelay));

            OnPlayerJumped.Invoke();
        }

        private IEnumerator GroundCheckCoroutine()
        {
            while (true)
            {
                _isGrounded = Physics2D.OverlapCircle(groundCheckerPoint.position, groundCheckerRadius, _groundLayer);
                yield return null;
            }
        }

        private IEnumerator GroundCheckCoroutineWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(GroundCheckCoroutine());
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckerPoint.position, groundCheckerRadius);
        }
    }
}