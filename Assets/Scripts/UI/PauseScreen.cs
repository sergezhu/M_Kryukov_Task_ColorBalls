using UnityEngine;

namespace UI
{
    public class PauseScreen : PopupScreen
    {
        public void Init()
        {
            base.Init();
        }
        
        protected override void OnShowBehaviour()
        {
            Debug.Log("PauseScreen Show");
        }

        protected override void OnHideBehaviour()
        {
            Debug.Log("PauseScreen Hide");
        }
    }
}
