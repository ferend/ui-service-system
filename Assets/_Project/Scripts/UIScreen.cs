using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Scripts
{
   [RequireComponent(typeof(GraphicRaycaster), typeof(Canvas))]
    public abstract class UIScreen : MonoBehaviour
    {
        private event Func<bool> _canHideDelegate;

        public enum VisibilityState
        {
            Interactable,
            Visible,
            Hidden
        }

        public event Action<UIScreen> OnReturn;

        public event Action<UIScreen> OnClose;

        private GraphicRaycaster _raycaster;
        protected Canvas _canvas;
        private Type _typeRef;
        private VisibilityState _state;

        public bool IsInteractable => isActiveAndEnabled && _raycaster.enabled;

        public VisibilityState Visibility => _state;
        public Type TypeRef => _typeRef;

        public virtual void Setup()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
            _canvas = GetComponent<Canvas>();
            _typeRef = GetType();
            Assert.IsNotNull(_raycaster);
            Assert.IsNotNull(_canvas);
        }

        public virtual void Dispose()
        {
            RemoveDelegate();
        }
        
        public virtual void SetVisibility(VisibilityState state)
        {
            switch (state)
            {
                case VisibilityState.Interactable:
                    _raycaster.enabled = true;
                    _canvas.enabled = true;

                    break;

                case VisibilityState.Visible:
                    _raycaster.enabled = false;
                    _canvas.enabled = true;

                    break;

                case VisibilityState.Hidden:
                    _raycaster.enabled = false;
                    _canvas.enabled = false;

                    break;
            }

            _state = state;
        }
        
        public async UniTask Close()
        {
            await WaitHideDelegate();
            OnClose?.Invoke(this);
            OnClose = null;
        }
        public async UniTask Return()
        {
            await WaitHideDelegate();
            OnReturn?.Invoke(this);
            OnReturn = null;
        }
        public void RemoveDelegate()
        {
            _canHideDelegate = null;
        }

        private async UniTask WaitHideDelegate()
        {
            if (_canHideDelegate == null)
            {
                return;
            }

            await UniTask.WaitUntil(() => _canHideDelegate != null && _canHideDelegate.Invoke());
        }
    }
}