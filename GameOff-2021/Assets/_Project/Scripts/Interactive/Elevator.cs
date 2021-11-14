using System;
using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Elevator : Controllable, IUITwoAxisControl
    {
        [SerializeField] private float speed;
        [SerializeField] private float maxHeight, minHeight;

        private bool _isWorking = false;
        private float _yDirection = 0f;
        private Vector3 _startPos;
        private float _maxAbsHeight, _minAbsHeight;

        private void Awake()
        {
            _startPos = transform.position;
            _maxAbsHeight = _startPos.y + maxHeight;
            _minAbsHeight = _startPos.y + minHeight;
        }

        private void Update()
        {
            if (!_isWorking)
                return;

            var yDir = transform.position.y + 0.1f * _yDirection;
            if (yDir < _maxAbsHeight && yDir > _minAbsHeight)
                transform.Translate(Vector2.up * speed * _yDirection * Time.deltaTime);
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
            _yDirection = -1f;
        }

        public void OnClick_RightBtn()
        {
            _isWorking = true;
            _yDirection = 1f;
        }

        public void OnClick_StopBtn()
        {
            Stop();
        }

        private void Stop()
        {
            _isWorking = false;
            _yDirection = 0f;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + Vector3.up * maxHeight, 0.5f);
                Gizmos.DrawWireSphere(transform.position + Vector3.up * minHeight, 0.5f);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_startPos + Vector3.up * maxHeight, 0.5f);
                Gizmos.DrawWireSphere(_startPos + Vector3.up * minHeight, 0.5f);
            }
        }
    }
}