using UnityEngine;

namespace Core
{
    public class Direction
    {
        private Vector3 _value;

        public Direction(Vector3 value)
        {
            _value = value;
        }

        public Vector3 Value => _value;
    }
}
