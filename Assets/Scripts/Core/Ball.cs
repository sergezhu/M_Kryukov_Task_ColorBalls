using System;
using UnityEngine;

namespace Core
{
    public class Ball : MovableBody
    {
        private bool _isMirrored;
        public override void Move()
        {
            var mirrorMultiplier = _isMirrored ? -1f : 1f;
            var direction = Settings.StartDirection + Settings.DirectionDeviation * mirrorMultiplier * Vector3.right;
            var velocity = Settings.StartSpeed * direction;
            var force = velocity * Time.fixedDeltaTime;
            Rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.TryGetComponent<Ball>(out var ball) || other.collider.TryGetComponent<Wall>(out var wall))
            {
                _isMirrored = !_isMirrored;
            }
        }
    }
}
