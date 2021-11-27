using System.Collections;
using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.NPC
{
    public class Turret : MonoBehaviour, IDestroyable
    {
        [Header("Turret Variables")] [SerializeField]
        private Transform shotPoint;

        [SerializeField] private float shootDelay = 1.5f;

        [Header("Proj Variables")] [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField] private float projectileFlySpeed;

        private Collider2D _collider2D;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(shootDelay);
                Shoot();
            }
        }


        private void Shoot()
        {
            Debug.Log("Turret shoot.");
            var proj = Instantiate(projectilePrefab, shotPoint.position, shotPoint.rotation).GetComponent<Projectile>();
            proj.OnSpawn(-shotPoint.right, projectileFlySpeed, _collider2D);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}