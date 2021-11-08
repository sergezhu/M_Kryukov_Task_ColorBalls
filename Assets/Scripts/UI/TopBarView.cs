using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TopBarView : MonoBehaviour
    {
        [SerializeField]
        private Slider _healthSlider;
        [SerializeField]
        private TMP_Text _heathText;
        [SerializeField]
        private TMP_Text _scoresText;
        [SerializeField]
        private TMP_Text _failsText;

        [Space]
        [SerializeField]
        private Player _player;

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
            _player.HealthChanged += OnHealthChanged;
            _player.ScoresChanged += OnScoresChanged;
            _player.FailsCountChanged += OnFailsCountChanged;
        }

        private void Unsubscribe()
        {
            _player.HealthChanged += OnHealthChanged;
            _player.ScoresChanged += OnScoresChanged;
            _player.FailsCountChanged += OnFailsCountChanged;
        }

        private void OnHealthChanged(int value)
        {
            var relativeHealth = (float)value / _player.MaxHeath;
            _healthSlider.value = relativeHealth;

            _heathText.text = $"{(Mathf.Clamp01(relativeHealth) * 100):f0}%";
        }

        private void OnScoresChanged(int value)
        {
            _scoresText.text = $"{value}";
        }

        private void OnFailsCountChanged(int value)
        {
            _failsText.text = $"{value}";
        }
    }
}
