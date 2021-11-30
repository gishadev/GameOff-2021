using Gisha.Effects.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.MainMenu
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        private void Start()
        {
            if (PlayerPrefs.HasKey("SavedTime"))
                timerText.text = "Time: " + PlayerPrefs.GetString("SavedTime");
        }

        public void OnClick_Exit()
        {
            SceneManager.LoadScene("MainMenu");
            AudioManager.Instance.PlaySFX("Hit");
        }
    }
}