using UnityEngine;
using System;
using System.Collections;
using Gisha.Effects.Audio;
using Gisha.GameOff_2021.Core;
using Gisha.GameOff_2021.Interactive;
using Gisha.GameOff_2021.Utilities;

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
        [SerializeField] private PhysicsMaterial2D maxFrictionMaterial;

        [Space] [SerializeField] private ParticleSystem footStepsEffect;
        [SerializeField] private ParticleSystem impactEffect;
        [SerializeField] private float rateOverTime;

        public Vector2 Velocity => _rb.velocity;
        public Action PlayerJumped { set; get; }
        private Action PlayerFell { set; get; }

        private float _coyoteCounter;
        private float _bufferCounter;
        private LayerMask _groundLayer;
        private bool _isGrounded = true;
        private float _hInput;

        private ParticleSystem.EmissionModule _emissionModule;
        private Rigidbody2D _rb;
        private Collider2D _coll;

        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _emissionModule = footStepsEffect.emission;
            _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        }

        private void Start()
        {
            StartCoroutine(GroundCheckCoroutine());
        }

        // Unsubscribing on return to this mode.
        private void OnEnable()
        {
            PlayerFell -= OnFellOnGroundInControl;
        }

        public override void Update()
        {
            HandleInput();

            _emissionModule.rateOverTimeMultiplier = Mathf.Abs(_hInput) > 0 ? rateOverTime : 0f;
        }

        public override void FixedUpdate()
        {
            _rb.velocity = new Vector2(_hInput * moveSpeed * Time.deltaTime, _rb.velocity.y);
        }

        public override void ResetOnBehaviourChange()
        {
            if (_isGrounded)
                _coll.sharedMaterial = maxFrictionMaterial;
            else
                PlayerFell += OnFellOnGroundInControl;

            ControllablesVisualizer.SpawnControllableVisuals(GameManager.ControllableList);
            PostProcessingController.SetControlPreset();
        }

        private void OnFellOnGroundInControl()
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.Play();
            _coll.sharedMaterial = maxFrictionMaterial;
            
            PlayerFell -= OnFellOnGroundInControl;
        }

        private void OnFellOnGround()
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Play();
            
            PlayerFell -= OnFellOnGround;
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

            PlayerFell += OnFellOnGround;
            PlayerJumped.Invoke();
            
            AudioManager.Instance.PlaySFX("secondJump");
        }

        private IEnumerator GroundCheckCoroutine()
        {
            bool once = false;
            while (true)
            {
                _isGrounded = Physics2D.OverlapCircle(groundCheckerPoint.position, groundCheckerRadius, _groundLayer);

                if (_isGrounded && !once)
                {
                    PlayerFell?.Invoke();
                    once = true;
                }

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