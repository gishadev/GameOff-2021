using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class Projectile : MonoBehaviour, IDestroyable
    {
        [SerializeField] private float lifeTime = 5f;

        private float _flySpeed;
        private Vector2 _flyDirection;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        public void OnSpawn(Vector2 direction, float flySpeed)
        {
            _flySpeed = flySpeed;
            _flyDirection = direction;
        }

        private void Update()
        {
            transform.Translate(_flyDirection * _flySpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                Destroy(gameObject);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}