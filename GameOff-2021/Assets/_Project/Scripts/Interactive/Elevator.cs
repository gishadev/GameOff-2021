using System;
using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Elevator : Controllable, IUITwoAxisControl
    {
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxDist, minDist;

        private bool _isWorking = false;
        private Vector2 _straightDir = Vector2.one;
        private Vector3 _startPos;
        private float _maxAbsYDist, _minAbsYDist;
        private float _maxAbsXDist, _minAbsXDist;

        private void Awake()
        {
            _startPos = transform.position;
            _maxAbsYDist = _startPos.y + maxDist * moveDirection.y + 0.02f;
            _minAbsYDist = _startPos.y + minDist * moveDirection.y - 0.02f;
            _maxAbsXDist = _startPos.x + maxDist * moveDirection.x + 0.02f;
            _minAbsXDist = _startPos.x + minDist * moveDirection.x - 0.02f;
        }

        private void Update()
        {
            if (!_isWorking)
                return;

            var sDir = (Vector2) transform.position + moveDirection * 0.1f * _straightDir;
            if (sDir.y < _maxAbsYDist && sDir.y > _minAbsYDist && sDir.x < _maxAbsXDist && sDir.x > _minAbsXDist)
                transform.Translate(moveDirection * moveSpeed * _straightDir * Time.deltaTime);
            else
                Stop();
        }

        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_RightBtn);
            InteractActions.Add(OnClick_StopBtn);
            InteractActions.Add(OnClick_LeftBtn);
        }

        public void OnClick_LeftBtn()
        {
            _isWorking = true;
            _straightDir = -Vector2.one;
        }

        public void OnClick_RightBtn()
        {
            _isWorking = true;
            _straightDir = Vector2.one;
        }

        public void OnClick_StopBtn()
        {
            Stop();
        }

        private void Stop()
        {
            _isWorking = false;
            _straightDir = Vector2.zero;
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