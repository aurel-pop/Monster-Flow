using System.Collections;
using UnityEngine;

namespace MonsterFlow.Objects.Player
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        public float playerDeathTime;
        private Animator _anim;

        private void Awake()
        {
            _anim = gameObject.GetComponent<Animator>();
            if (_anim == null) Debug.Log("Cannot find 'Animator' component");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Tomato"))
                FaintSleep();
        }

        private void FaintSleep()
        {
            StartCoroutine(AnimateForSeconds("Player_Sleep-Fainting", playerDeathTime));
        }

        private IEnumerator AnimateForSeconds(string animationName, float seconds)
        {
            _anim.Play(animationName);
            yield return new WaitForSeconds(seconds);
            _anim.Play("Idle");
            yield return null;
        }
    }
}