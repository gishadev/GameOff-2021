using System;
using Gisha.GameOff_2021.Core;
using Gisha.GameOff_2021.Player;
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

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

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
            RaycastAttack();
        }

        private void RaycastAttack()
        {
            RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, attackRayRadius,
                moveDirection * _straightDir, attackRayLength);

            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Player"))
                    hitInfo.collider.GetComponent<PlayerController>().Die();
            }
        }

        public void Destroy()
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