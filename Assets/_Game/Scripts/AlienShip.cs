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
                Tooltip("Random chance to shoot bullet")]
        private float randomFireRate = .95f;

        [field: SerializeField] 
        private float delayBetweenTryingToShoot = 1f;

        private IEnumerator _randomFireBullet;
        
        public Action destroyShipEvent;
        public Action gameOverEnterEvent;


        private void Start()
        {
            _randomFireBullet = RandomFireBullets();

            StartCoroutine(_randomFireBullet);
        }

        public void EliminateThisShip()
        {
            destroyShipEvent();
            StopCoroutine(_randomFireBullet);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GameOverWall"))
            {
                gameOverEnterEvent();
            }
        }

        private IEnumerator RandomFireBullets()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(delayBetweenTryingToShoot);

                if (Random.value > randomFireRate)
                    ObjectPooling.Instance.SpawnFromPool(canonTransform.position, canonTransform.rotation); 
            }
        }
    }
}
