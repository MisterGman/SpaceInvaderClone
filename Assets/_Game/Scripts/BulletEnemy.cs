using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts
{
    public class BulletEnemy : MonoBehaviour, IBulletCooldown
    {
        [field: SerializeField] 
        private float speed = 4f;

        [field: SerializeField] 
        private Rigidbody2D bulletRb;

        private const float CooldownTime = 2f;
        private IEnumerator _cooldownUntilDestroyBullet;

        private void OnEnable()
        {
            bulletRb.velocity = -transform.up * speed;
            
            _cooldownUntilDestroyBullet = CooldownUntilBulletDestroyed();
            StartCoroutine(_cooldownUntilDestroyBullet);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.GetComponent<PlayerShip>().DestroyShip();
            StopCoroutine(_cooldownUntilDestroyBullet);
            ObjectPooling.Instance.ReturnToPool(gameObject);
        }

        public IEnumerator CooldownUntilBulletDestroyed()
        {
            yield return new WaitForSeconds(CooldownTime);
            
            ObjectPooling.Instance.ReturnToPool(gameObject);
        }
    }
}
