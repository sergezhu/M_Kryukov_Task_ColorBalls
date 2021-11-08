using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public abstract class MovableBody : MonoBehaviour, IMovableBody
    {
        [SerializeField]
        private MovableSettings _settings;
        public MovableSettings Settings => _settings;
        protected Rigidbody Rigidbody { get; private set; }
        private MeshRenderer MeshRenderer { get; set; }

        public abstract void Move();

        public void Init(MovableSettings settings)
        {
            _settings = settings;
            
            var velocity = Settings.StartSpeed * Settings.StartDirection;
            Rigidbody.velocity = velocity;

            MeshRenderer.material.color = Settings.Color;
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            MeshRenderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
