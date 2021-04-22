using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "ShipSpawnerInfo")]
    public class ShipSpawnerSO : ScriptableObject
    {
        public GameObject alienShipPrefab;
        public GameObject columnContainer;
        
        [Space]
        
        [Range(1, 5)]
        public int numberInColumn = 5;
        
        [Range(1, 8)]
        public int numberOfColumn = 8;
        
        [Space]
        
        public float yPadding = .8f;
        public float xPadding = .8f;
    }
}
