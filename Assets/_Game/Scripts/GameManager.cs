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
        private AlienBoard alienBoard;

        [field: SerializeField] 
        private UiManager uiManager;

        [field: SerializeField] 
        private GameOverWall gameOverWall;
        
        private SaveLoad _saveLoad;

        private bool _isGameOverActice;
        
        private void Start()
        {
            _saveLoad = new SaveLoad();
            _saveLoad.Load(uiManager);
            Time.timeScale = 1;

            alienBoard.Init();
            playerShip.Init();

            gameOverWall.GameOverEnterCallback = RestartTheScene;
            gameOverWall.GameOverEnterCallback = alienBoard.GameOverCallback =
                playerShip.GameOverCallback = RestartTheScene;
        }

        private void RestartTheScene()
        {
            if(_isGameOverActice) return;
            
            _isGameOverActice = true;
            StartCoroutine(DelayBeforeReloading());
        }

        private IEnumerator DelayBeforeReloading()
        {
            Time.timeScale = 0;
            
            yield return new WaitForSecondsRealtime(3f);
            
            _saveLoad.Save(uiManager.CurrentHighScore);
            SceneManager.LoadScene(1);
        }
    }
}
