using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterFlow.System
{
    public class SwitchScenes : MonoBehaviour
    {
        private BackgroundMusicSwitcher _backgroundMusicSwitcher;

        private void Start()
        {
            _backgroundMusicSwitcher = FindObjectOfType<BackgroundMusicSwitcher>();

            if (_backgroundMusicSwitcher == null)
                print("Could not find BackgroundMusicSwitcher script");
        }

        private void Update()
        {
            if (Input.touchCount <= 0 && !Input.anyKey) return;
            if (_backgroundMusicSwitcher != null)
                _backgroundMusicSwitcher.SwitchBackgroundMusic();

            SceneManager.LoadScene("Game");
        }
    }
}
