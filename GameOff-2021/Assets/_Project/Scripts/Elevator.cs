using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class Elevator : Controllable
    {
        private bool _isRising = false;

        private void Update()
        {
            if (!_isRising)
                return;

            transform.Translate(Vector2.up * 0.1f * Time.deltaTime);
        }

        public override void InteractAction()
        {
            _isRising = true;
        }
    }
}