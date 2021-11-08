using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct RandomBounds
    {
        [SerializeField]
        private float _min;
        [SerializeField]
        private float _max;

        public RandomBounds(float min, float max)
        {
            _min = Mathf.Min(min, max);
            _max = Mathf.Max(min, max);
        }

        public float Min => _min;
        public float Max => _max;
        public float Random => UnityEngine.Random.Range(_min, _max);
    }
}