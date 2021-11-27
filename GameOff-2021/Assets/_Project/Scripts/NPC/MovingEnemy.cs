using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class MovingEnemy : MonoBehaviour, IDestroyable
    {
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxDist, minDist;

        private Vector2 _straightDir = Vector2.one;
        private Vector3 _startPos;
        private float _maxAbsYDist, _minAbsYDist;
        private float _maxAbsXDist, _minAbsXDist;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _startPos = transform.position;
            _maxAbsYDist = _startPos.y + maxDist * moveDirection.y + 0.25f;
            _minAbsYDist = _startPos.y + minDist * moveDirection.y - 0.1f;
            _maxAbsXDist = _startPos.x + maxDist * moveDirection.x + 0.1f;
            _minAbsXDist = _startPos.x + minDist * moveDirection.x - 0.25f;
        }

        private void FixedUpdate()
        {
            var sDir = (Vector2) transform.position + moveDirection * 0.1f * _straightDir;
            if (sDir.y < _maxAbsYDist && sDir.y > _minAbsYDist && sDir.x < _maxAbsXDist && sDir.x > _minAbsXDist)
                _rb.velocity = moveDirection * moveSpeed * _straightDir * Time.deltaTime;
            else
                _straightDir *= -1;
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
        }
    }
}