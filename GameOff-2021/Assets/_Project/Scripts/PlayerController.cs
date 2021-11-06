using System.Collections;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 500f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.5f;

        [Header("Ground Checker")] [SerializeField]
        private Transform groundCheckerPoint;

        [SerializeField] private float groundCheckerRadius = 0.25f;
        [SerializeField] private float afterJumpCheckDelay = 0.1f;
        [Space] [SerializeField] private ControlBehaviour _controlBehaviour;

        private bool _isControlMode;
        private float _coyoteCounter;
        private float _bufferCounter;
        private LayerMask _groundLayer;
        private bool _isGrounded = true;
        private float _hInput;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        }

        private void Start()
        {
            StartCoroutine(GroundCheckCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                _isControlMode = !_isControlMode;

            if (_isControlMode)
            {
                _controlBehaviour.HandleInput();
                return;
            }

            HandleInput();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_hInput * moveSpeed * Time.deltaTime, _rb.velocity.y);
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

            // Making full jump.
            if (_bufferCounter > 0 && _coyoteCounter > 0)
            {
                MakeFullJump();

                _bufferCounter = 0;
                StopAllCoroutines();
                StartCoroutine(GroundCheckCoroutineWithDelay(afterJumpCheckDelay));
            }

            // Making half jump.
            if (Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0)
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        private void MakeFullJump()
        {
            _isGrounded = false;
            _coyoteCounter = 0;

            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
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

    [System.Serializable]
    internal class ControlBehaviour
    {
        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var coll = Physics2D.OverlapCircleAll(clickPoint, 0.1f);

                for (int i = 0; i < coll.Length; i++)
                {
                    coll[i].gameObject.TryGetComponent(out Controllable controllable);
                    
                    if (controllable != null)
                        controllable.InteractAction();
                }
            }
        }
    }
}