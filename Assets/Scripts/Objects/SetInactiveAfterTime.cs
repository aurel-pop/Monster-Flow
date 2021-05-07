using UnityEngine;

namespace MonsterFlow.Objects
{
    public class SetInactiveAfterTime : MonoBehaviour
    {
        public float maxTime;
        private float _time;

        private void Update()
        {
            SetObjectInactiveAfterTime();
        }

        private void SetObjectInactiveAfterTime()
        {
            _time += Time.deltaTime;
            if (_time > maxTime) gameObject.SetActive(false);
        }
    }
}