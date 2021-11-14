using Gisha.GameOff_2021.Interactive;
using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    internal class ControlBehaviour : PlayerBehaviour
    {
        [SerializeField] private PhysicsMaterial2D zeroFrictionMaterial;

        private Collider2D _coll;

        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
        }

        public override void Update()
        {
           // HandleInput();
        }

        public override void FixedUpdate()
        {
        }

        public override void ResetOnBehaviourChange()
        {
            _coll.sharedMaterial = zeroFrictionMaterial;
            
            ControllablesVisualizer.RemoveControllableUIElements();
        }

        // public void HandleInput()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //         var coll = Physics2D.OverlapCircleAll(clickPoint, 0.1f);
        //
        //         for (int i = 0; i < coll.Length; i++)
        //         {
        //             coll[i].gameObject.TryGetComponent(out Controllable controllable);
        //
        //             if (controllable != null)
        //                 controllable.InteractAction();
        //         }
        //     }
        // }
    }
}