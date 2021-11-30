using Gisha.Effects.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gisha.GameOff_2021.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject creditsMenu;
        [SerializeField] private Image sfxImage;
        [SerializeField] private Image musicImage;
        
        [SerializeField] private Sprite sfxOn, sfxOff;
        
        
        public void OnClick_Play()
        {
            SceneManager.LoadScene("Game");

            MakeClickSound();
        }

        public void OnClick_SFX()
        {
            AudioManager.Instance.SetSFXVolume(AudioManager.Instance.IsSfxMuted ? 1f : 0f);
            sfxImage.sprite = AudioManager.Instance.IsSfxMuted ? sfxOff : sfxOn;
            
            MakeClickSound();
        }
        
        public void OnClick_Music()
        {
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.IsMusicMuted ? 0.3f : 0f);
            musicImage.sprite = AudioManager.Instance.IsMusicMuted ? sfxOff : sfxOn;
            
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