using UnityEngine;

namespace MonsterFlow.System
{
    public class DontDestroy : MonoBehaviour
    {
        private static bool _created;

        private void Awake()
        {
            if (!_created)
            {
                DontDestroyOnLoad(gameObject);
                _created = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}