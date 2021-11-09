using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        public abstract void Update();

        public abstract void FixedUpdate();

        public abstract void ResetOnBehaviourChange();
    }
}