using UnityEngine;

namespace MonsterFlow.Objects.Enemy
{
    public class RandomRotator : MonoBehaviour
    {
        public float rotationSpeed;

        private void FixedUpdate()
        {
            RotateObject();
        }

        private void RotateObject()
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
    }
}