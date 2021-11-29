using Gisha.GameOff_2021.Interactive;
using Gisha.GameOff_2021.Utilities;
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
        }

        public override void FixedUpdate()
        {
        }

        public override void ResetOnBehaviourChange()
        {
            _coll.sharedMaterial = zeroFrictionMaterial;
            
            ControllablesVisualizer.RemoveAllControllableUIElements();
            PostProcessingController.SetDefaultPreset();
        }
    }
}