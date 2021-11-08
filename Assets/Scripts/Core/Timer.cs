using System;
using UnityEngine;

namespace Core
{
    public class Timer
    {
        private Action _callback;
        private float _remainedTime;
        private bool _isActive;

        public Timer()
        {
            _remainedTime = 0;
            _isActive = false;
        }

        public void Start(float duration, Action callback)
        {
            if (duration < 0)
                throw new InvalidOperationException();
            
            _callback = callback;
            _remainedTime = duration;
            _isActive = true;
        }

        public void Cancel()
        {
            _callback = null;
            _isActive = false;
        }

        public void Tick()
        {
            if (_isActive == false)
                return;

            _remainedTime -= Time.deltaTime;

            if (_remainedTime <= 0)
            {
                _isActive = false;
                _callback?.Invoke();
            }
        }
    }
}