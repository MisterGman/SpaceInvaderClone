using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public class AlienBoard : MonoBehaviour
    {
        #region Variables

        [field: SerializeField] 
        private UiManager uiManager;
        
        [field: SerializeField, 
                Tooltip("Scriptable object with data to create a board")]
        private ShipSpawnerSO shipSpawnerInfo;
        
        [field: SerializeField, 
                Tooltip("Horizontal step for each rate")] 
        private float moveDistance = .4f;
        
        [field: SerializeField, 
                Tooltip("Vertical step when bound is reached")] 
        private float moveDistanceVertical = .4f;

        [field: SerializeField, 
                Tooltip("Standard move rate")] 
        private float defaultMoveRate = .5f;

        /// <summary>
        /// All columns of alien ships
        /// </summary>
        private readonly List<AlienColumnContainer> _alienColumns = new List<AlienColumnContainer>();

        private Transform _transform;

        public Action gameOverEvent;
        
        #endregion

        public void Init()
        {
            _transform = transform;

            SpawnShips();
            
            foreach (var column in _alienColumns) 
                column.lastShipDestroyedEvent = LastShipDestroyedCallback;
            
            StartCoroutine(MoveBoard());
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="column">Column that was destroyed recently</param>
        private void LastShipDestroyedCallback(AlienColumnContainer column)
        {
            var index = _alienColumns.FindIndex(container => container.Index == column.Index);
            
            _alienColumns.RemoveAt(index);

            if (_alienColumns.Count != 0) return;

            gameOverEvent();
        }

        /// <summary>
        /// Spawn the whole board
        /// </summary>
        private void SpawnShips()
        {
            RecenterBoard(shipSpawnerInfo.xPadding * shipSpawnerInfo.numberOfColumn - 1);

            var currentPositionX = Vector3.zero;

            for (int i = 0; i < shipSpawnerInfo.numberOfColumn; i++)
            {
                var currentPositionY = Vector3.zero;
            
                //Create a new column
                Transform newColumnTransform = Instantiate(shipSpawnerInfo.columnContainer, 
                    Vector3.zero, Quaternion.identity, _transform).transform;
                
                newColumnTransform.name = $"Column {i}";
                
                AlienColumnContainer newColumnComponent =
                    newColumnTransform.GetComponent<AlienColumnContainer>();

                newColumnComponent.Index = i;
                
                //Add this column to the board's list
                _alienColumns.Add(newColumnComponent);
            
                for (int j = 0; j < shipSpawnerInfo.numberInColumn; j++)
                {
                    //Create a new ship
                    GameObject newShipGameObject = Instantiate(shipSpawnerInfo.alienShipPrefab,
                        currentPositionY, Quaternion.identity, newColumnTransform);
                    
                    AlienShip newShipComponent = newShipGameObject.GetComponent<AlienShip>();
                    
                    
                    //Add the event handler to the parent column
                    newShipComponent.destroyShipEvent += newColumnComponent.EliminateLowestShipHandler;
                    newShipComponent.destroyShipEvent += uiManager.SetScoreText;
                    newShipComponent.gameOverEnterEvent += gameOverEvent;
                    
                    //Increase the count in the list
                    newColumnComponent.EnemiesInColumn++;

                    //Set the next ship lower by the Y padding
                    currentPositionY.y -= shipSpawnerInfo.yPadding;
                }
                
                //Set the next column to the right by the X padding
                newColumnTransform.localPosition = currentPositionX;
                currentPositionX.x += shipSpawnerInfo.xPadding;
            }
        }

        /// <summary>
        /// Recenter the board depending on width of column
        /// </summary>
        /// <param name="width"></param>
        private void RecenterBoard(float width)
        {
            var transformPosition = _transform.position;
            transformPosition.x -= width / 2;
            _transform.position = transformPosition;
        }

        private IEnumerator MoveBoard()
        {
            while (true) //TODO: While not game over
            {
                _transform.position += Vector3.right * moveDistance;
                //
                yield return new WaitForSeconds(defaultMoveRate);

                if (_alienColumns[0].transform.position.x <= -3f ||
                    _alienColumns[_alienColumns.Count - 1].transform.position.x >= 3f) //TODO: Declare numbers here
                {
                    moveDistance = -moveDistance;
                    _transform.position += Vector3.down * moveDistanceVertical;
                    yield return new WaitForSeconds(defaultMoveRate);
                }
            }
        }
    }
}
