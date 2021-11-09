using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerBehaviour[] _behaviours;

        private void Awake()
        {
            // Adding behaviours.
            _behaviours = new PlayerBehaviour[2];
            _behaviours[0] = GetComponent<MovementBehaviour>();
            _behaviours[1] = GetComponent<ControlBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SwitchBehaviour();
        }

        public void SwitchBehaviour()
        {
            PlayerBehaviour temp = _behaviours[0];
            temp.ResetOnBehaviourChange();

            // Enabling right one behaviour.
            _behaviours[0].enabled = false;
            _behaviours[1].enabled = true;

            // Switching behaviours.
            _behaviours[0] = _behaviours[1];
            _behaviours[1] = temp;

            Debug.Log("Behaviour was switched!");
        }
    }
}