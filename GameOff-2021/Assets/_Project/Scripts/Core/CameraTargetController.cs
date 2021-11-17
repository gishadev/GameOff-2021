using UnityEngine;

namespace Gisha.GameOff_2021.Core
{
    /// <summary>
    /// This script should be attached to the player.
    /// </summary>
    public class CameraTargetController : MonoBehaviour
    {
        [SerializeField] private float aheadAmount, aheadSpeed;

        private void Update()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                float xPos = Mathf.Lerp(transform.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"),
                    Time.deltaTime * aheadSpeed);

                transform.localPosition = new Vector3(xPos, transform.position.y,
                    transform.position.z);
            }
        }
    }
}