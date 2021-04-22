using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class GameOverWall : MonoBehaviour
    {
        private Action _gameOverEnterCallback;

        public Action GameOverEnterCallback
        {
            get => _gameOverEnterCallback;
            set => _gameOverEnterCallback = value;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Alien"))
            {
                _gameOverEnterCallback();
            }
        }
    }
}
