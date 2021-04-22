using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class AlienShip : MonoBehaviour
    {
        [field: SerializeField] 
        private Transform canonTransform;
        
        [field: SerializeField, 
                Tooltip("Random chance to shoot bullet"), Range(.1f, 1f)]
        private float randomFireRate = .95f;

        [field: SerializeField] 
        private float delayBetweenTryingToShoot = 1f;

        private IEnumerator _randomFireBullet;
        
        private Action _destroyShipCallback;
        private Action _gameOverEnterCallback;

        public Action DestroyShipCallback
        {
            get => _destroyShipCallback;
            set => _destroyShipCallback = value;
        }

        public Action GameOverEnterCallback
        {
            get => _gameOverEnterCallback;
            set => _gameOverEnterCallback = value;
        }

        private void Start()
        {
            _randomFireBullet = RandomFireBullets();

            StartCoroutine(_randomFireBullet);
        }

        public void EliminateThisShip()
        {
            _destroyShipCallback();
            StopCoroutine(_randomFireBullet);
            Destroy(gameObject);
        }

        /// <summary>
        /// Shoots bullets with random rate
        /// </summary>
        /// <returns></returns>
        private IEnumerator RandomFireBullets()
        {
            yield return new WaitForSecondsRealtime(delayBetweenTryingToShoot);

            while (true)
            {
                if (Random.value > randomFireRate)
                    ObjectPooling.Instance.SpawnFromPool(canonTransform.position, canonTransform.rotation); 
                
                yield return new WaitForSecondsRealtime(delayBetweenTryingToShoot);
            }
        }
    }
}
