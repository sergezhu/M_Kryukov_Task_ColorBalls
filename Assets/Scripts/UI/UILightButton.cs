using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public abstract class UILightButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action Click;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_button.interactable)
            {
                OnPointerClickHandler(eventData);
                Click?.Invoke();
            }
        }

        protected abstract void OnPointerClickHandler(PointerEventData eventData);
    }
}