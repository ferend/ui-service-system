using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public interface IUIService
    {
        UIScreen ActiveScreen { get; }
        public UniTask<TScreen> OpenScreen<TScreen>() where TScreen : UIScreen;
        public TScreen GetScreen<TScreen>() where TScreen : UIScreen;
        UIScreen GetScreenByType(Type type);
        
    }

    public class MissingScreenComponentException : System.Exception
    {
        private GameObject _subject;

        public MissingScreenComponentException(GameObject subject)
        {
            _subject = subject;
        }

        public override string Message => $"No '{nameof(UIScreen)}' component found in '{_subject.name}'";
    }
}