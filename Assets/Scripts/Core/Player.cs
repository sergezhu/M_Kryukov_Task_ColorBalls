using System;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public event Action<int> ScoresChanged;
        public event Action<int> HealthChanged;
        public event Action<int> FailsCountChanged;
        public event Action<DiedData> Died;
    
        [SerializeField][Range(100, 2000)]
        private int _maxHeath;

        [Space]
        [SerializeField]
        private FailHandler _failHandler;
        [SerializeField]
        private AwardHandler _awardHandler;
        [SerializeField]
        private SaveSystem _saveSystem;

        private int _health;
        private int _scores;
        private bool _isDied;

        public int MaxHeath => _maxHeath;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _isDied = false;
            _health = _maxHeath;
            _scores = 0;
            
            ScoresChanged?.Invoke(_scores);
            HealthChanged?.Invoke(_health);
            FailsCountChanged?.Invoke(0);
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _failHandler.MovableLost += OnMovableLost;
            _awardHandler.AwardReceived += OnAwardReceived;
        }

        private void Unsubscribe()
        {
            _failHandler.MovableLost -= OnMovableLost;
            _awardHandler.AwardReceived -= OnAwardReceived;
        }

        private void OnAwardReceived(int awardPoints)
        {
            TakeAward(awardPoints);
            ScoresChanged?.Invoke(_scores);
        }

        private void TakeAward(int awardPoints)
        {
            _scores += awardPoints;
        }

        private void OnMovableLost(MovableBody movableBody)
        {
            FailsCountChanged?.Invoke(_failHandler.DetectedFailBodiesCount);
            
            var damage = (int)movableBody.Settings.DamagePoints;
            TakeDamage(damage);
        }

        private void TakeDamage(int damage)
        {
            if (_health <= 0 || _isDied)
                throw new InvalidOperationException();

            _health -= damage;
            HealthChanged?.Invoke(_health);

            if (_health <= 0)
            {
                _isDied = true;

                HandleDied();
                Unsubscribe();
            }
        }

        private void HandleDied()
        {
            _saveSystem.Load();
            _saveSystem.CurrentProgress.AddResult(_scores);

            var diedData = new DiedData(_saveSystem.CurrentProgress.GetLastResult(), _saveSystem.CurrentProgress.GetBestResult());
            Died?.Invoke(diedData);
        }
    }
}
