using UnityEngine;

namespace Gisha.GameOff_2021.Core
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed;

        private float _viewableWidth;
        private LevelBounds _levelBounds;

        public void SetLevelBounds(LevelBounds levelBounds)
        {
            _levelBounds = levelBounds;
        }

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

            if (xPos + _viewableWidth > _levelBounds.RightBound.position.x)
                result = _levelBounds.RightBound.position.x - _viewableWidth;

            if (xPos - _viewableWidth < _levelBounds.LeftBound.position.x)
                result = _levelBounds.LeftBound.position.x + _viewableWidth;

            return result;
        }
    }
}