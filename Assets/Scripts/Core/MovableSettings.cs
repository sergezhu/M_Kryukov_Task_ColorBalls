using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class MovableSettings
    {
        [SerializeField][GUIReadOnly]
        private float _startSpeed;
        [SerializeField][GUIReadOnly]
        private float _directionDeviation;
        [SerializeField][GUIReadOnly]
        private Vector3 _startDirection;
        [SerializeField][GUIReadOnly]
        private float _awardPoints;
        [SerializeField][GUIReadOnly]
        private float _damagePoints;
        [SerializeField][GUIReadOnly]
        private Color _color;

        public MovableSettings(float startSpeed, float directionDeviation, Vector3 startDirection, float awardPoints, float damagePoints, Color color)
        {
            _startSpeed = startSpeed;
            _directionDeviation = directionDeviation;
            _startDirection = startDirection;
            _awardPoints = awardPoints;
            _damagePoints = damagePoints;
            _color = color;
        }

        public float StartSpeed => _startSpeed;
        public Vector3 StartDirection => _startDirection;
        public float DirectionDeviation => _directionDeviation;
        public float AwardPoints => _awardPoints;
        public float DamagePoints => _damagePoints;
        public Color Color => _color;
    }
}
