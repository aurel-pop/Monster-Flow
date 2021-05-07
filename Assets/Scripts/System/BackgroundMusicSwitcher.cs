using UnityEngine;

namespace MonsterFlow.System
{
    public class BackgroundMusicSwitcher : MonoBehaviour
    {
        public AudioClip backgroundMusicGame;
        public float volumeDownStep;
        public float volumeUpStep;
        public float volumeCrossover;
        public float gameMusicVolume;
        private AudioSource _audioSource;
        private bool _turnVolumeDown;
        private bool _turnVolumeUp = true;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _audioSource.volume = gameMusicVolume;

            if (backgroundMusicGame == null)
                print("Could not find BackgroundMusicGame AudioClip");
        }

        private void Update()
        {
            SetVolume();
        }

        private void SetVolume()
        {
            if (_turnVolumeUp)
            {
                _audioSource.volume += volumeUpStep * Time.deltaTime;

                if (_audioSource.volume >= gameMusicVolume) _turnVolumeUp = false;
            }

            if (!_turnVolumeDown) return;
            _audioSource.volume -= volumeDownStep * Time.deltaTime;

            if (!(_audioSource.volume < volumeCrossover)) return;
            _turnVolumeDown = false;

            _audioSource.clip = backgroundMusicGame;
            _audioSource.Play();

            _turnVolumeUp = true;
        }

        public void SwitchBackgroundMusic()
        {
            _turnVolumeDown = true;
        }
    }
}