using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class TapParticle : MonoBehaviour
    {
        [SerializeField] [Min(0)]
        private float _destroyDelay = 10f;

        private Color _mainColor;
        private Color _trailColor;
        
        private ParticleSystem ParticleSystem { get; set; }
        private ParticleSystemRenderer ParticleSystemRender { get; set; }
        private MeshRenderer MeshRenderer { get; set; }

        private void Awake()
        {
            MeshRenderer = GetComponent<MeshRenderer>();
            ParticleSystem = GetComponent<ParticleSystem>();
            ParticleSystemRender = GetComponent<ParticleSystemRenderer>();
        }

        public void Init(Color color, Transform groundPlane)
        {
            _mainColor = color;
            MeshRenderer.material.color = _mainColor;
            
            ParticleSystem.collision.SetPlane(0, groundPlane);

            var colorOverLifeTime = ParticleSystem.colorOverLifetime;
            var c1 = _mainColor;
            var c2 = _mainColor;
            c2.a = 0;

            colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(c1, c2);

            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }
    }
}
