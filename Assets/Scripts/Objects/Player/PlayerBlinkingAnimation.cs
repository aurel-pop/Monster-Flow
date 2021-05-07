using UnityEngine;

namespace MonsterFlow.Objects.Player
{
    public class PlayerBlinkingAnimation : MonoBehaviour
    {
        private Animator _anim;
        private float _blinkingTimer;
        private float _randomBlinkingRange;
        private float _randomRange;
        private float _timer;

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _randomRange = Random.Range(2.5f, 4f);
            _randomBlinkingRange = Random.Range(0.19f, 0.25f);
        }

        private void Update()
        {
            BlinkEyes();
        }

        private void BlinkEyes()
        {
            _timer += Time.deltaTime;

            if (_timer > _randomRange)
            {
                _anim.SetBool("IsClosingEyes", true);
                _timer = 0;
                _randomRange = Random.Range(2.5f, 4f);
            }

            if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Blinking")) return;
            _blinkingTimer += Time.deltaTime;

            if (!(_blinkingTimer > _randomBlinkingRange)) return;
            _anim.SetBool("IsClosingEyes", false);
            _blinkingTimer = 0;
            _timer = 0;
            _randomBlinkingRange = Random.Range(0.19f, 0.25f);
        }
    }
}