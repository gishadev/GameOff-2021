using System;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed;

        [Header("Bounds")] [SerializeField] private Transform leftBound;
        [SerializeField] private Transform rightBound;

        private float _viewableWidth;

        private void Start()
        {
            _viewableWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        }

        private void FixedUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            Vector3 newPosition = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
            newPosition.x = AttachXToBounds(newPosition.x);
            
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }

        private float AttachXToBounds(float xPos)
        {
            float result = xPos;

            if (xPos + _viewableWidth > rightBound.position.x)
                result = rightBound.position.x - _viewableWidth;

            if (xPos - _viewableWidth < leftBound.position.x)
                result = leftBound.position.x + _viewableWidth;

            return result;
        }
    }
}