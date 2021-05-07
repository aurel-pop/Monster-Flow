using UnityEngine;

namespace MonsterFlow.Objects
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public bool dontDestroyOnLoad;
        public float destroyAfterSeconds;
        private float _time;

        private void Awake()
        {
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            DestroyObjectAtSeconds();
        }

        private void DestroyObjectAtSeconds()
        {
            _time += Time.deltaTime;
            if (destroyAfterSeconds > 0 && _time > destroyAfterSeconds) Destroy(gameObject);
        }
    }
}