using UnityEngine;

namespace UI
{
    public abstract class PopupScreen : MonoBehaviour
    {
        private bool _isShow;
        private bool _isInitialized;
        private GameObject _gameObject;

        private void Awake()
        {
            Init();
        }

        protected void Init()
        {
            if (_isInitialized)
                return;
            
            _isShow = false;
            _gameObject = gameObject;
            _gameObject.SetActive(false);
            _isInitialized = true;
        }

        public void Show()
        {
            if (_isShow)
                return;

            _isShow = true;
            _gameObject.SetActive(true);
            OnShowBehaviour();
            
            Time.timeScale = 0;
        }
    
        public void Hide()
        {
            if (_isShow == false)
                return;

            _isShow = false;
            OnHideBehaviour();
            _gameObject.SetActive(false);
            
            Time.timeScale = 1f;
        }

        protected abstract void OnShowBehaviour();
        protected abstract void OnHideBehaviour();
    }
}