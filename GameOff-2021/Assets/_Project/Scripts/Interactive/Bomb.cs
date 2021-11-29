using Gisha.Effects.Audio;
using Gisha.Effects.VFX;
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
            AudioManager.Instance.PlaySFX("ControlUse2");
        }

        private void Explode()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent(out IDestroyable destroyable))
                    destroyable.Destroy();
            }
            
            VFXManager.Instance.Emit("L_Explosion", transform.position);
            AudioManager.Instance.PlaySFX("deathEnemy1");
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}