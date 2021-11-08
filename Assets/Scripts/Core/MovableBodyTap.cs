using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    [RequireComponent(typeof(MovableBody))]
    public class MovableBodyTap : MonoBehaviour, IPointerClickHandler
    {
        public event Action<MovableBody> MovableDestroyed;
        public event Action<Vector3, Color> ParticleRequested;

        private SpawnedObjectsHolder _spawnedObjectsHolder;
        private MovableBody _movableBody;
        private Color _color;

        private void Awake()
        {
            _movableBody = GetComponent<MovableBody>();
        }

        public void Init(SpawnedObjectsHolder spawnedObjectsHolder)
        {
            _spawnedObjectsHolder = spawnedObjectsHolder;
            _color = _movableBody.Settings.Color;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            RequestParticle();
            DestroyOriginal();
        }

        private void DestroyOriginal()
        {
            MovableDestroyed?.Invoke(_movableBody);
            Destroy(gameObject, 0.1f);
        }

        private void RequestParticle()
        {
            ParticleRequested?.Invoke(_movableBody.transform.position, _color);
        }
    }
}
