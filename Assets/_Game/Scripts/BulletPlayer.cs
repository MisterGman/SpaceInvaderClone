using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts
{
    public class BulletPlayer : MonoBehaviour, IBulletCooldown
    {
        [field: SerializeField] 
        private float speed = 4f;

        [field: SerializeField] 
        private Rigidbody2D bulletRb;

        private const float CooldownTime = 2.5f;
        
        private IEnumerator _cooldownUntilDestroyBullet;

        public Action AllowToShoot;

        private void OnEnable()
        {
            bulletRb.velocity = transform.up * speed;

            _cooldownUntilDestroyBullet = CooldownUntilBulletDestroyed();
            StartCoroutine(_cooldownUntilDestroyBullet);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Alien")) return;
            
            other.GetComponent<AlienShip>().EliminateThisShip();
            StartCoroutine(_cooldownUntilDestroyBullet);
            DestroyBullet();
        }

        private void DestroyBullet()
        {
            AllowToShoot();
            Destroy(gameObject);
        }
        
        public IEnumerator CooldownUntilBulletDestroyed()
        {
            yield return new WaitForSeconds(CooldownTime);
            
            DestroyBullet();
        }
    }
}
