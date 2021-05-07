using UnityEngine;

namespace MonsterFlow.Objects
{
    public class YellowMonster : MonoBehaviour
    {
        private static readonly int IsSleeping = Animator.StringToHash("IsSleeping");
        public float animationSpeed;
        public AudioClip[] eatingSounds;
        public float lowPitchRange;
        public float highPitchRange;
        private Animator _animator;
        private AudioSource _audioSource;
        private int _lastSound;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.speed = animationSpeed;
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Tomato")) return;
            _animator.SetTrigger(IsSleeping);
            _audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);

            if (eatingSounds.Length <= 0) return;
            var i = Random.Range(0, eatingSounds.Length);
            while (_lastSound == i)
                i = Random.Range(0, eatingSounds.Length);

            _audioSource.PlayOneShot(eatingSounds[i]);
            _lastSound = i;
        }
    }
}