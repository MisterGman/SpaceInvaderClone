using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    public class ObjectPooling : MonoBehaviour
    {
        [field: SerializeField]
        private GameObject bulletPrefab;
        
        [field: SerializeField]
        private int initialSize = 10;

        private readonly Queue<GameObject> _objectFromPool = new Queue<GameObject>();

        public static ObjectPooling Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            for (int i = 0; i < initialSize; i++)
            {
                var go = Instantiate(bulletPrefab);
                _objectFromPool.Enqueue(go);
                go.SetActive(false);
            }
        }

        public void SpawnFromPool(Vector3 pos, Quaternion rot)
        {
            if (_objectFromPool.Count > 0)
            {
                var spawnObj = _objectFromPool.Dequeue();

                spawnObj.SetActive(true);
                spawnObj.transform.position = pos;
                spawnObj.transform.rotation = rot;

            }
            else
            {
                var spawnObj = Instantiate(bulletPrefab);
                spawnObj.transform.position = pos;
                spawnObj.transform.rotation = rot;
            }
        }

        public void ReturnToPool(GameObject go)
        {
            _objectFromPool.Enqueue(go);
            go.SetActive(false);
        }
    }
}