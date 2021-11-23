using UnityEngine;

namespace Gisha.GameOff_2021.Core
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float followSpeed;

        private float _viewableWidth;
        private float _viewableHeight;
        private LevelManager _currentLevel;

        public void SetLevel(LevelManager levelManager)
        {
            _currentLevel = levelManager;
        }

        private void Start()
        {
            _viewableHeight = Camera.main.orthographicSize;
            _viewableWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        }

        private void FixedUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            Vector3 newPosition = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
            newPosition = AttachPositionToBounds(newPosition.x, newPosition.y);

            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }

        private Vector3 AttachPositionToBounds(float xPos, float yPos)
        {
            float xResult = xPos;
            float yResult = yPos;

            if (xPos + _viewableWidth > _currentLevel.RightBound.position.x)
                xResult = _currentLevel.RightBound.position.x - _viewableWidth;

            if (xPos - _viewableWidth < _currentLevel.LeftBound.position.x)
                xResult = _currentLevel.LeftBound.position.x + _viewableWidth;

            if (yPos + _viewableHeight > _currentLevel.RightBound.position.y)
                yResult = _currentLevel.RightBound.position.y - _viewableHeight;

            if (yPos - _viewableHeight < _currentLevel.LeftBound.position.y)
                yResult = _currentLevel.LeftBound.position.y + _viewableHeight;

            return new Vector3(xResult, yResult, transform.position.z);
        }
    }
}