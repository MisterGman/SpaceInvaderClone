using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] 
        private PlayerShip playerShip;
        
        [field: SerializeField]
        private SaveLoad saveLoad;

        [field: SerializeField] 
        private AlienBoard alienBoard;

        private void Start()
        {
            Time.timeScale = 1;

            saveLoad.Init();
            alienBoard.Init();
            alienBoard.gameOverEvent = playerShip.gameOverEvent = RestartTheScene;
        }

        private void RestartTheScene()
        {
            StartCoroutine(DelayBeforeReloading());
        }

        private IEnumerator DelayBeforeReloading()
        {
            Time.timeScale = 0;
            
            yield return new WaitForSecondsRealtime(3f);
            
            saveLoad.Save();
            SceneManager.LoadScene(1);
        }
    }
}
