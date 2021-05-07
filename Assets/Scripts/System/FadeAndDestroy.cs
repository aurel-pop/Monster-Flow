using TMPro;
using UnityEngine;

namespace MonsterFlow.System
{
    public class FadeAndDestroy : MonoBehaviour
    {
        public float waitBeforeFade;
        public float fadeTime;
        public Vector2 fadeMovement;
        public float fadeRotation;
        public Vector3 fadeScaling;
        private float _percentageDone;

        private SpriteRenderer _renderer;
        private TextMeshPro _textMesh;

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _textMesh = GetComponentInChildren<TextMeshPro>();

            if (_renderer is null && _textMesh is null) Destroy(gameObject);

            if (waitBeforeFade > 0) _percentageDone -= waitBeforeFade / fadeTime;
        }

        private void Update()
        {
            FadeAndDestroyObject();
        }

        private void FadeAndDestroyObject()
        {
            var timePercentage = Time.deltaTime / fadeTime;

            if (_percentageDone >= 0f)
            {
                if (_renderer) _renderer.color -= new Color(0, 0, 0, timePercentage);
                if (_textMesh) _textMesh.color -= new Color(0, 0, 0, timePercentage);
                var transformRef = transform;
                transformRef.Translate(fadeMovement * timePercentage);
                transformRef.Rotate(0, 0, fadeRotation * timePercentage);
                transformRef.localScale += fadeScaling * timePercentage;
            }

            _percentageDone += timePercentage;
            if (_percentageDone >= 1f) Destroy(gameObject);
        }
    }
}