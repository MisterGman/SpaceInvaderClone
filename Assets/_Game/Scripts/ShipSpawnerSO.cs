using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "ShipSpawnerInfo")]
    public class ShipSpawnerSO : ScriptableObject
    {
        [field: SerializeField]
        private GameObject alienShipPrefab;
        
         [field: SerializeField]
        private GameObject columnContainerPrefab;
        
        [Space]
        
        [field: SerializeField, Range(1, 5)]
        private int numberInColumn = 5;
        
        [field: SerializeField, Range(1, 8)]
        private int numberOfColumn = 8;
        
        [Space]
        
        [field: SerializeField, Range(.5f, 1f)]
        private float yPadding = .8f;
        
        [field: SerializeField, Range(.5f, 1f)]
        private float xPadding = .8f;


        public GameObject AlienShipPrefab => alienShipPrefab;

        public GameObject ColumnContainerPrefab => columnContainerPrefab;

        public int NumberInColumn => numberInColumn;

        public int NumberOfColumn => numberOfColumn;

        public float YPadding => yPadding;

        public float XPadding => xPadding;
    }
}
