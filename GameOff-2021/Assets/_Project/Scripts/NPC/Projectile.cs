using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class Projectile : MonoBehaviour
    {
        private float _flySpeed;
        private Vector2 _flyDirection;

        public void OnSpawn(Vector2 direction, float flySpeed)
        {
            _flySpeed = flySpeed;
            _flyDirection = direction;
        }

        private void Update()
        {
            transform.Translate(_flyDirection * _flySpeed * Time.deltaTime, Space.World);
        }
    }
}