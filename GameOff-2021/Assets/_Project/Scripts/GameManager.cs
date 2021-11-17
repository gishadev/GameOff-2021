using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private Queue<LevelBounds> _levelBoundsQueue = new Queue<LevelBounds>();

        private CameraFollowController _cameraFollow;

        private void Awake()
        {
            Instance = this;
            _cameraFollow = Camera.main.GetComponent<CameraFollowController>();
        }

        private void Start()
        {
            InsertAllLevelBoundsToQueue();
            _cameraFollow.SetLevelBounds(_levelBoundsQueue.Peek());
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