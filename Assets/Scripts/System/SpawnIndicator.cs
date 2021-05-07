using UnityEngine;

namespace MonsterFlow.System
{
    public class SpawnIndicator : MonoBehaviour
    {
        private void Start()
        {
        }

        public void Setup(float rotation, Color hue = default)
        {
            Transform transformRef;
            (transformRef = transform).Rotate(0, 0, rotation);

            var angleInQuarter = rotation % 90f;
        }
    }
}