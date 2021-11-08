using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AwardHandler : MonoBehaviour
    {
        public event Action<int> AwardReceived;
    
        private List<MovableBodyTap> _movableBodyTaps;
        private int _award;

        private void Awake()
        {
            _movableBodyTaps = new List<MovableBodyTap>();
        }

        public void AddAndSubscribeMovableBodyTap(MovableBodyTap movableBodyTap)
        {
            if (_movableBodyTaps.Contains(movableBodyTap))
                return;
        
            _movableBodyTaps.Add(movableBodyTap);
            SubscribeOnMovableBodyTap(movableBodyTap);
        }

        private void SubscribeOnMovableBodyTap(MovableBodyTap movableBodyTap)
        {
            movableBodyTap.MovableDestroyed += OnMovableDestroyed;
        }
    
        private void UnsubscribeOnMovableBodyTap(MovableBodyTap movableBodyTap)
        {
            movableBodyTap.MovableDestroyed -= OnMovableDestroyed;
        }

        private void OnMovableDestroyed(MovableBody movableBody)
        {
            var movableBodyTap = movableBody.GetComponent<MovableBodyTap>();

            if (_movableBodyTaps.Contains(movableBodyTap) == false)
                return;

            _movableBodyTaps.Remove(movableBodyTap);
            UnsubscribeOnMovableBodyTap(movableBodyTap);

            AwardReceived?.Invoke((int) movableBody.Settings.AwardPoints);
        }
    }
}
