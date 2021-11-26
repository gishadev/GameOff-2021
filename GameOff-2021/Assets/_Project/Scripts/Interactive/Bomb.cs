using System;
using System.Collections.Generic;
using System.Linq;
using Gisha.GameOff_2021.Core;
using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Bomb : Controllable, IUITriggerControl
    {
        [SerializeField] private float damageRadius = 5f;


        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_TriggerBtn);
        }

        public void OnClick_TriggerBtn()
        {
            Explode();
            Destroy(gameObject);
        }

        private void Explode()
        {
            Debug.Log("Boom!");

            List<IDestroyable> destroyables = new List<IDestroyable>();
            var colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent(out IDestroyable destroyable)) 
                    destroyable.Destroy();
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}