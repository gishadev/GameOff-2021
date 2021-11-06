using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed;

        private void FixedUpdate()
        {
            Vector3 newPosition = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }
    }
}