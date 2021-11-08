using Core;
using UnityEngine;

namespace UI
{
    public class ScreensHandler : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        
        [Space]
        [SerializeField]
        private PauseScreen _pauseScreen;
        [SerializeField]
        private GameOverScreen _gameOverScreen;
        
        [Space]
        [SerializeField]
        private MenuButton _menuButton;
        [SerializeField]
        private BackgroundButton _backgroundButton;
    
        private void OnEnable()
        {
            _player.Died += OnDied;
            _menuButton.Click += OnMenuButtonClick;
            _backgroundButton.Click += OnBackgroundButtonClick;
        }
    
        private void OnDisable()
        {
            _player.Died -= OnDied;
            _menuButton.Click -= OnMenuButtonClick;
            _backgroundButton.Click -= OnBackgroundButtonClick;
        }

        private void OnDied(DiedData data)
        {
            _gameOverScreen.Init(data.LastResult, data.BestResult);
            _gameOverScreen.Show();
        }

        private void OnMenuButtonClick()
        {
            _pauseScreen.Init();
            _pauseScreen.Show();
        }

        private void OnBackgroundButtonClick()
        {
            _pauseScreen.Hide();
        }
    }
}
