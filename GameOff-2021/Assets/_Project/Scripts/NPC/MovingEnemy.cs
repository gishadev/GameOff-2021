using Gisha.GameOff_2021.Core;
using Gisha.GameOff_2021.Player;
using TMPro;
using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class MovingEnemy : MonoBehaviour, IDestroyable
    {
        [Header("Movement")] [SerializeField] private Vector2 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxDist, minDist;
        [Header("Attack")] [SerializeField] private float attackRayLength = 0.5f;
        [Header("Attack")] [SerializeField] private float attackRayRadius = 0.25f;

        private Vector2 _straightDir = Vector2.one;
        private Vector3 _startPos;
        private float _maxAbsYDist, _minAbsYDist;
        private float _maxAbsXDist, _minAbsXDist;
        private int _playerMask;
        private RaycastHit2D _attackHitInfo;

        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();

            _playerMask = 1 << LayerMask.NameToLayer("Player");

            _startPos = transform.position;
            _maxAbsYDist = _startPos.y + maxDist * moveDirection.y + 0.1f;
            _minAbsYDist = _startPos.y + minDist * moveDirection.y - 0.1f;
            _maxAbsXDist = _startPos.x + maxDist * moveDirection.x + 0.1f;
            _minAbsXDist = _startPos.x + minDist * moveDirection.x - 0.1f;
        }

        private void FixedUpdate()
        {
            var sDir = (Vector2) transform.position + moveDirection * _straightDir;
            if (sDir.y < _maxAbsYDist && sDir.y > _minAbsYDist && sDir.x < _maxAbsXDist && sDir.x > _minAbsXDist)
                _rb.velocity = moveDirection * moveSpeed * _straightDir * Time.deltaTime;
            else
                _straightDir *= -1;
        }

        private void Update()
        {
            if (CheckAreaForTarget())
            {
                _animator.SetBool("IsTargetDetected", true);
                RaycastAttack();
            }
            else
                _animator.SetBool("IsTargetDetected", false);

            HandleSpriteFlipping();
        }

        private void HandleSpriteFlipping()
        {
            _spriteRenderer.flipX = _straightDir.x < 0;
        }

        private void RaycastAttack()
        {
            _attackHitInfo = Physics2D.CircleCast(transform.position, attackRayRadius,
                moveDirection * _straightDir, attackRayLength, _playerMask);

            if (_attackHitInfo.collider != null)
                _animator.SetTrigger("Attack");
        }

        private void Attack()
        {
            if (_attackHitInfo.collider != null)
                _attackHitInfo.collider.GetComponent<PlayerController>().Die();
        }

        private bool CheckAreaForTarget()
        {
            // Max is right, min is left.
            var maxPoint = (Vector2) _startPos + moveDirection * maxDist;
            var minPoint = (Vector2) _startPos + moveDirection * minDist;

            RaycastHit2D maxHitInfo = Physics2D.CircleCast(transform.position, attackRayRadius, Vector2.right
                , maxPoint.x - transform.position.x, _playerMask);

            RaycastHit2D minHitInfo = Physics2D.CircleCast(transform.position, attackRayRadius, Vector2.left
                , transform.position.x - minPoint.x, _playerMask);

            // Debug.DrawRay(transform.position, Vector2.left * (transform.position.x - minPoint.x),Color.yellow);
            // Debug.DrawRay(transform.position, Vector2.right * (maxPoint.x - transform.position.x),Color.blue);

            if (maxHitInfo.collider != null)
            {
                _straightDir = Vector2.one;
                return true;
            }

            if (minHitInfo.collider != null)
            {
                _straightDir = -Vector2.one;
                return true;
            }

            return false;
        }

        public void Destroy()
        {
            _animator.SetTrigger("Die");
        }

        public void DestroyOnFinishAnimation()
        {
            Destroy(gameObject);
        }
        
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere((Vector2) transform.position + moveDirection * maxDist, 0.5f);
                Gizmos.DrawWireSphere((Vector2) transform.position + moveDirection * minDist, 0.5f);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere((Vector2) _startPos + moveDirection * maxDist, 0.5f);
                Gizmos.DrawWireSphere((Vector2) _startPos + moveDirection * minDist, 0.5f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, moveDirection * _straightDir * attackRayLength);
        }
    }
}