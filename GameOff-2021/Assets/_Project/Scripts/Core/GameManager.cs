using System.Collections.Generic;
using System.Linq;
using Gisha.GameOff_2021.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string[] locationScenes;
        private static GameManager Instance { get; set; }

        private Queue<LevelBounds> _levelBoundsQueue = new Queue<LevelBounds>();

        private CameraFollowController _cameraFollow;
        private PlayerController _player;

        private void Awake()
        {
            Instance = this;
            _cameraFollow = Camera.main.GetComponent<CameraFollowController>();
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private void Start()
        {
            InsertAllLevelBoundsToQueue();
            _cameraFollow.SetLevelBounds(_levelBoundsQueue.Peek());
        }

        private void LateUpdate()
        {
            if (_player.transform.position.x > _levelBoundsQueue.Peek().RightBound.position.x)
            {
                _levelBoundsQueue.Dequeue();
                _cameraFollow.SetLevelBounds(_levelBoundsQueue.Peek());
            }
        }

        public static void RestartLocation()
        {
            SceneManager.LoadScene("Game");

            var scenes = Instance.locationScenes;
            foreach (var s in scenes)
                SceneManager.LoadScene(s, LoadSceneMode.Additive);
        }

        private void InsertAllLevelBoundsToQueue()
        {
            // Get all scenes.
            var scenes = new Scene[SceneManager.sceneCount];
            for (int i = 0; i < SceneManager.sceneCount; i++)
                scenes[i] = SceneManager.GetSceneAt(i);

            // Filter scenes.
            scenes = scenes
                .Where(x => x.name.StartsWith("Level"))
                .OrderBy(
                    x => int.Parse(x.name.Split('_').Last())
                )
                .ToArray();

            // Get Level Bounds.
            var allBounds = FindObjectsOfType<LevelBounds>();

            // Insert in queue.
            foreach (var s in scenes)
            foreach (var b in allBounds)
                if (b.gameObject.scene == s)
                    _levelBoundsQueue.Enqueue(b);
        }
    }
}