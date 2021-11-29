using Gisha.Effects.Audio;
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

        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
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

            HandleElevatorMovement();
        }

        private void FixedUpdate()
        {
            if (!_isWorking)
                return;
            
            //HandlePassengersMovement();
        }

        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_RightBtn);
            InteractActions.Add(OnClick_StopBtn);
            InteractActions.Add(OnClick_LeftBtn);
        }

        #region OnClick Entries

        public void OnClick_LeftBtn()
        {
            _isWorking = true;
            _straightDir = -Vector2.one;
            
            AudioManager.Instance.PlaySFX("ControlUse2");
        }

        public void OnClick_RightBtn()
        {
            _isWorking = true;
            _straightDir = Vector2.one;
            AudioManager.Instance.PlaySFX("ControlUse2");
        }

        public void OnClick_StopBtn()
        {
            Stop();
            AudioManager.Instance.PlaySFX("ControlUse3");
        }

        #endregion

        private void HandleElevatorMovement()
        {
            var sDir = (Vector2) transform.position + moveDirection * 0.1f * _straightDir;
            if (sDir.y < _maxAbsYDist && sDir.y > _minAbsYDist && sDir.x < _maxAbsXDist && sDir.x > _minAbsXDist)
            {
                transform.Translate(moveDirection * moveSpeed * _straightDir * Time.deltaTime);
                _animator.SetBool("IsMoving", true);
            }
            else
                Stop();
        }

        private void Stop()
        {
            _isWorking = false;
            _straightDir = Vector2.zero;
            
            _animator.SetBool("IsMoving", false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.transform.parent = transform;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            other.transform.parent = null;
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