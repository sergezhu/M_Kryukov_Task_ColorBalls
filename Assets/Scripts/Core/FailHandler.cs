using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class FailHandler : MonoBehaviour
    {
        public event Action<int> CountChanged;
        public event Action<MovableBody> MovableLost; 
        
        [SerializeField][Min(0.1f)]
        private float _liveAfterFail = 0.5f;
        [SerializeField][GUIReadOnly]
        private int _detectedFailBodiesCount;
        
        private List<MovableBody> _failBodies;

        public int DetectedFailBodiesCount => _detectedFailBodiesCount;

        private void Awake()
        {
            _failBodies = new List<MovableBody>();
            _detectedFailBodiesCount = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MovableBody movableBody))
            {
                if (_failBodies.Contains(movableBody) == false)
                {
                    _detectedFailBodiesCount++;
                    _failBodies.Add(movableBody);
                    CountChanged?.Invoke(_detectedFailBodiesCount);
                    MovableLost?.Invoke(movableBody);
                    
                    StartCoroutine(DelayedDestroy(movableBody));
                }
            }
        }

        private IEnumerator DelayedDestroy(MovableBody movableBody)
        {
            yield return new WaitForSeconds(_liveAfterFail);
            _failBodies.Remove(movableBody);
            
            Destroy(((MonoBehaviour)movableBody).gameObject);
        }
    }
}
