using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Bomb : Controllable, IUITriggerControl
    {
        protected override void OnAddInteractActions()
        {
            InteractActions.Add(OnClick_TriggerBtn);
        }
        
        public void OnClick_TriggerBtn()
        {
            Debug.Log("Boom!");
            Destroy(gameObject);
        }
    }
}
