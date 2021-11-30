using Gisha.Effects.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gisha.GameOff_2021.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject creditsMenu;

        public void OnClick_Play()
        {
            SceneManager.LoadScene("Game");
            
            MakeClickSound();
        }

        public void OnClick_Settings()
        {
            settingsMenu.SetActive(true);
            
            MakeClickSound();
        }

        public void OnClick_Credits()
        {
            creditsMenu.SetActive(true);
            
            MakeClickSound();
        }

        public void OnClick_CloseSubmenu()
        {
            creditsMenu.SetActive(false);
            settingsMenu.SetActive(false);
            
            MakeClickSound();
        }

        public void OnClick_Quit()
        {
            Application.Quit();
            
            MakeClickSound();
        }

        private void MakeClickSound()
        {
            AudioManager.Instance.PlaySFX("Hit");
        }
    }
}