using MonsterFlow.System;
using UnityEngine;

namespace MonsterFlow.Objects.Enemy
{
    public class MoveToOrigin : MonoBehaviour
    {
        public float speed;
        private Vector3 _moveDirection;

        private void Start()
        {
            _moveDirection = (Vector3.zero - transform.position).normalized;
        }

        private void Update()
        {
            MoveObjectToOrigin();
        }

        private void MoveObjectToOrigin()
        {
            transform.Translate(_moveDirection *
                                (speed * GameController.Instance.CurrentSpeedMultiplier * Time.deltaTime));
        }
    }
}