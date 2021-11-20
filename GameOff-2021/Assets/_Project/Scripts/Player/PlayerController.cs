using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerBehaviour[] _behaviours;

        private int _lives = 3;

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

        private void SwitchBehaviour()
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

        private void Die()
        {
            _lives--;
            if (_lives > 0)
            {
                GameManager.RespawnOnLevel(this);
            }
            else
                GameManager.RestartLocation();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
                Die();
        }
    }
}