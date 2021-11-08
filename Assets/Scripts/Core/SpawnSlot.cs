using System;
using UnityEngine;

namespace Core
{
    public class SpawnSlot
    {
        private Vector3 _relativePosition;
        private Vector3 _offset;
        private IMovableBody _containedObject;
        private Transform _containedObjectTransform;

        public SpawnSlot(Vector3 relativePosition)
        {
            _relativePosition = relativePosition;
            _containedObject = null;
        }

        public Vector3 RelativePosition => _relativePosition;
        public bool IsFree => _containedObject is null;

        public void Put(IMovableBody movableObject)
        {
            _containedObject = movableObject;
            _containedObjectTransform = ((MonoBehaviour) movableObject).transform;
            _offset = _containedObjectTransform.position - _relativePosition;
        }

        public void Clear()
        {
            _containedObject = null;
            _containedObjectTransform = null;
        }

        public bool CanClear()
        {
            if (_containedObject == null || _containedObjectTransform == null)
                return true;

            return Mathf.Abs(_containedObjectTransform.position.z - _offset.z - _relativePosition.z) > _containedObjectTransform.localScale.z;
        }
    }
}