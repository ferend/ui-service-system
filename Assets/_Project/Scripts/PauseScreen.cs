using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class PauseScreen : UIScreen
    {
        private void Awake()
        {
            Setup();
            OnClose += screen => Debug.Log("Close the screen");
        }
    }
}