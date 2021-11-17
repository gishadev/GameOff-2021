using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}