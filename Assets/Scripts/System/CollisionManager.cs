using UnityEngine;

namespace MonsterFlow.System
{
    public class CollisionManager : MonoBehaviour
    {
        public GameObject explosion;
        public int scoreValue;

        private void Awake()
        {
            if (GameController.Instance == null) Debug.Log("Cannot find 'GameController' script");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) GameController.Instance.GameOver();

            if (other.gameObject.CompareTag("YellowMonster"))
            {
                GameController.Instance.UpdateScore(1);
                if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}