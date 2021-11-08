using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverScreen : PopupScreen
    {
        [SerializeField]
        private TMP_Text _lastResultText;
        [SerializeField]
        private TMP_Text _bestResultText;

        private int _lastResult;
        private int _bestResult;

        public void Init(int lastResult, int bestResult)
        {
            base.Init();
            
            _lastResult = lastResult;
            _bestResult = bestResult;
        }
        
        protected override void OnShowBehaviour()
        {
            _lastResultText.text = $"{_lastResult}";
            _bestResultText.text = $"{_bestResult}";
        }

        protected override void OnHideBehaviour()
        {
        }
    }
}