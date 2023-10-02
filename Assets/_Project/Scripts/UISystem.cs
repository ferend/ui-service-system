using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

namespace _Project.Scripts
{
    public class UISystem : MonoBehaviour, IUIService
    { 
        private ServiceLocator _serviceLocator;

        private UIScreen _activeScreen;

        private PauseScreen _pauseScreen;
        
        private void Start()
        {
            _serviceLocator = ServiceLocator.Global;
            _serviceLocator.RegisterService<IUIService>(this);
            _pauseScreen = GetScreen<PauseScreen>();
        }

        public void OpenPauseMenu()
        {
            if (_pauseScreen != null)
            {
                if (_activeScreen == _pauseScreen)
                {
                    // If it is, close it
                    _activeScreen.SetVisibility(UIScreen.VisibilityState.Hidden);
                    _activeScreen.Close();
                    _activeScreen = null;
                }
                else
                {
                    _pauseScreen.SetVisibility(UIScreen.VisibilityState.Visible);
                    _activeScreen = _pauseScreen;
                }
            }
            else
            {
                Debug.LogError("PauseScreen not found in the hierarchy.");
            }
        }
        
        
        private void OnDestroy()
        {
            _serviceLocator.UnregisterService<IUIService>();
        }

        public UIScreen ActiveScreen => _activeScreen;

        public UniTask<TScreen> OpenScreen<TScreen>() where TScreen : UIScreen
        {
            if (_activeScreen != null)
            {
                _activeScreen.SetVisibility(UIScreen.VisibilityState.Hidden);
            }

            // Find the specified screen by type
            TScreen screen = GetScreen<TScreen>();
            
            if (screen != null)
            {
                screen.SetVisibility(UIScreen.VisibilityState.Interactable);
                _activeScreen = screen;
            }

            return UniTask.FromResult(screen);
        }

        public TScreen GetScreen<TScreen>() where TScreen : UIScreen
        {
            TScreen screen = FindObjectOfType<TScreen>();

            if (screen == null)
            {
                Debug.LogError($"Screen of type {typeof(TScreen).Name} not found in the hierarchy.");
                return null;
            }

            return screen;
        }

        public UIScreen GetScreenByType(Type type)
        {
            UIScreen screen = FindObjectOfType(type) as UIScreen;

            if (screen == null)
            {
                Debug.LogError($"Screen of type {type.Name} not found in the hierarchy.");
                return null;
            }

            return screen;
        }

        void OnGUI()
        { 
            int originalFontSize = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = 44;
            GUI.Label(new Rect(10, 10, 200, 200), "Pause screen active status " + _pauseScreen.Visibility);
            GUI.skin.label.fontSize = originalFontSize;

        }

    }
}