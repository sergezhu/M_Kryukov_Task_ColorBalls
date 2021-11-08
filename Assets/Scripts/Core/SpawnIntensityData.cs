using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct SpawnIntensityData
    {
        [SerializeField][Range(.1f, 10f)]
        private float _waveDuration;

        [SerializeField][Range(.1f, 10f)]
        private float _spawnFrequency;

        public float WaveDuration => _waveDuration;
        public float SpawnFrequency => _spawnFrequency;
    }
}