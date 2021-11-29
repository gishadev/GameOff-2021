using System;
using System.Collections.Generic;
using System.Linq;
using Gisha.Effects.Audio;
using Gisha.GameOff_2021.Interactive;
using Gisha.GameOff_2021.Player;
using Gisha.GameOff_2021.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LocationData[] locations;
        private static GameManager Instance { get; set; }

        /// <summary>
        /// Action on level change, returns previous scene.
        /// </summary>
        public static Action<LevelManager> LevelChanged;

        private Queue<LevelManager> _levelManagersQueue = new Queue<LevelManager>();
        private LevelManager CurrentLevel => _levelManagersQueue.Peek();

        public static List<Controllable> ControllableList { get; set; }

        private static int _currentLocationIndex = 0;

        private CameraFollowController _cameraFollow;
        private PlayerController _player;

        private bool _oncePassed = false;

        private void Awake()
        {
            Instance = this;
            _cameraFollow = Camera.main.GetComponent<CameraFollowController>();
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            
#if !UNITY_EDITOR
            LoadLocation(_currentLocationIndex);
#endif
        }

        private void Start()
        {
            ControllableList = FindObjectsOfType<Controllable>().ToList();
            InsertAllLevelsToQueue();
            _cameraFollow.SetLevel(CurrentLevel);

            _player.transform.position = CurrentLevel.Spawnpoint.position;
        }

        private void LateUpdate()
        {
            // If player is out from the level side > moving to next level. 
            if (IsPassedLevel(_player.transform.position) && !_oncePassed)
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

        private void LoadLocation(int index)
        {
            Timer.Restart();

            for (int i = 0; i < locations[index].LevelsCount; i++)
            {
                var s = $"_Project/Scenes/Location_1/Level_{i + 1}";
                SceneManager.LoadScene(s, LoadSceneMode.Additive);
            }
        }

        public static void RestartLocation()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public static void RespawnOnLevel(PlayerController player)
        {
            player.transform.position = Instance.CurrentLevel.Spawnpoint.position;
            Instance.CurrentLevel.Spawnpoint.GetComponent<Animator>().SetTrigger("Spawn");

            AudioManager.Instance.PlaySFX("respawn3");
        }

        private void MoveToNextLevel()
        {
            // Check if the level was last one.
            if (_levelManagersQueue.Count < 2)
            {
                Debug.Log("<color=green>Last level finished. Moving to the next location.</color>");
                _currentLocationIndex++;
                _oncePassed = true;

                // Loading final scene for now.
                if (_currentLocationIndex + 1 > locations.Length)
                    SceneManager.LoadScene(1);
                else
                    LoadLocation(_currentLocationIndex);

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

    [Serializable]
    public class LocationData
    {
        [SerializeField] private string locationName;
        [SerializeField] private int levelsCount;

        public int LevelsCount => levelsCount;
    }
}