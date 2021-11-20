using UnityEngine;

namespace Gisha.GameOff_2021.Core
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed;

        private float _viewableWidth;
        private LevelManager _currentLevel;

        public void SetLevel(LevelManager levelManager)
        {
            _currentLevel = levelManager;
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

            if (xPos + _viewableWidth > _currentLevel.RightBound.position.x)
                result = _currentLevel.RightBound.position.x - _viewableWidth;

            if (xPos - _viewableWidth < _currentLevel.LeftBound.position.x)
                result = _currentLevel.LeftBound.position.x + _viewableWidth;

            return result;
        }
    }
}