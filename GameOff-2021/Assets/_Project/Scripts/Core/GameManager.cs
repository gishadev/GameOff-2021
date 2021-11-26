using System;
using System.Collections.Generic;
using System.Linq;
using Gisha.GameOff_2021.Interactive;
using Gisha.GameOff_2021.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string[] locationScenes;
        private static GameManager Instance { get; set; }

        /// <summary>
        /// Action on level change, returns previous scene.
        /// </summary>
        public static Action<LevelManager> LevelChanged;

        private Queue<LevelManager> _levelManagersQueue = new Queue<LevelManager>();
        private LevelManager CurrentLevel => _levelManagersQueue.Peek();

        public static List<Controllable> ControllableList { get; set; }
        private CameraFollowController _cameraFollow;
        private PlayerController _player;

        private void Awake()
        {
            Instance = this;
            _cameraFollow = Camera.main.GetComponent<CameraFollowController>();
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
#if !UNITY_EDITOR
            LoadLocation();
#endif
        }

        private void Start()
        {
            ControllableList = FindObjectsOfType<Controllable>().ToList();
            InsertAllLevelsToQueue();
            _cameraFollow.SetLevel(CurrentLevel);
        }

        private void LateUpdate()
        {
            // If player is out from the level side > moving to next level. 
            if (IsPassedLevel(_player.transform.position))
                MoveToNextLevel();
        }

        private bool IsPassedLevel(Vector3 playerPos)
        {
            // If player is out from the level side > moving to next level. 
            if (!CurrentLevel.IsVerticalExit)
            {
                if (!CurrentLevel.IsReversedExit && playerPos.x > CurrentLevel.RightBound.position.x)
                    return true;
                if (CurrentLevel.IsReversedExit && playerPos.x < CurrentLevel.LeftBound.position.x)
                    return true;
            }
            else
            {
                if (!CurrentLevel.IsReversedExit && playerPos.y > CurrentLevel.RightBound.position.y)
                    return true;
                if (CurrentLevel.IsReversedExit && playerPos.y < CurrentLevel.LeftBound.position.y)
                    return true;
            }

            return false;
        }

        private void LoadLocation()
        {
            foreach (var s in locationScenes)
                SceneManager.LoadScene(s, LoadSceneMode.Additive);
        }

        public static void RestartLocation()
        {
            SceneManager.LoadScene("Game");

            var scenes = Instance.locationScenes;
            foreach (var s in scenes)
                SceneManager.LoadScene(s, LoadSceneMode.Additive);
        }

        public static void RespawnOnLevel(PlayerController player)
        {
            player.transform.position = Instance.CurrentLevel.Spawnpoint.position;
        }

        private void MoveToNextLevel()
        {
            // Check if the level was last one.
            if (_levelManagersQueue.Count < 2)
            {
                Debug.Log("<color=green>Last level finished. Moving to the next location.</color>");
                // Loading final scene for now.
                SceneManager.LoadScene(1);
                return;
            }

            // Move to next level.
            LevelChanged?.Invoke(CurrentLevel);
            _levelManagersQueue.Dequeue();
            _cameraFollow.SetLevel(CurrentLevel);
        }

        private void InsertAllLevelsToQueue()
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
            var allBounds = FindObjectsOfType<LevelManager>();

            // Insert in queue.
            foreach (var s in scenes)
            foreach (var b in allBounds)
                if (b.gameObject.scene == s)
                    _levelManagersQueue.Enqueue(b);
        }
    }
}