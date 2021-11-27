using System;
using System.Linq;
using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class Projectile : MonoBehaviour, IDestroyable
    {
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private float raycastLength;

        private float _flySpeed;
        private Vector2 _flyDirection;

        private Collider2D _turretCollider;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        public void OnSpawn(Vector2 direction, float flySpeed, Collider2D turretCollider)
        {
            _flySpeed = flySpeed;
            _flyDirection = direction;
            _turretCollider = turretCollider;
        }

        private void Update()
        {
            transform.Translate(_flyDirection * _flySpeed * Time.deltaTime, Space.World);

            ForwardRaycast();
        }

        private void ForwardRaycast()
        {
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(transform.position,_flyDirection, raycastLength)
                .Where(x => x.collider != _turretCollider && x.collider != _collider)
                .ToArray();
            
            if (hitInfo.Length > 0) 
                Destroy(gameObject);
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _flyDirection * raycastLength);
        }
    }
}