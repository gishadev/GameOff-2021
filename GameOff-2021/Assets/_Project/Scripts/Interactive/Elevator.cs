using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Elevator : Controllable, IUITwoAxisControl
    {
        [SerializeField] private float speed;

        private bool _isWorking = false;
        private float _yDirection = 0f;

        private void Update()
        {
            if (!_isWorking)
                return;

            transform.Translate(Vector2.up * speed * _yDirection * Time.deltaTime);
        }

        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_RightBtn);
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
    }
}