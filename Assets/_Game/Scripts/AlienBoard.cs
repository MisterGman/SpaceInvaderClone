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
                Tooltip("Distance from the center for player to stop")]    
        private float horizontalBoundDistance = 3f;
        
        [field: SerializeField, 
                Tooltip("Horizontal step for each rate"), 
                Range(.1f, 1f)] 
        private float moveDistance = .4f;
        
        [field: SerializeField, 
                Tooltip("Vertical step when bound is reached"), 
                Range(.1f, 1f)] 
        private float moveDistanceVertical = .4f;

        [field: SerializeField, 
                Tooltip("Standard move rate"), 
                Range(.1f, 1f)] 
        private float defaultMoveRate = .5f;

        /// <summary>
        /// All columns of alien ships
        /// </summary>
        private readonly List<AlienColumnContainer> _alienColumns = new List<AlienColumnContainer>();

        private Transform _transform;

        private Action gameOverCallback;

        public Action GameOverCallback
        {
            get => gameOverCallback;
            set => gameOverCallback = value;
        }
        
        #endregion

        public void Init()
        {
            _transform = transform;

            SpawnShips();
            
            foreach (var column in _alienColumns) 
                column.LastShipDestroyedEvent = LastShipDestroyedCallback;
            
            StartCoroutine(MoveBoard());
        }

        /// <summary>
        /// Checks if all columns were destroyed
        /// </summary>
        /// <param name="column">Column that was destroyed recently</param>
        private void LastShipDestroyedCallback(AlienColumnContainer column)
        {
            for (int i = 0; i < _alienColumns.Count; i++)
            {
                if (_alienColumns[i].Index != column.Index) continue;
                
                _alienColumns.RemoveAt(i);
                break;
            }
            
            if (_alienColumns.Count != 0) return;

            gameOverCallback();
        }

        private void ForceGameOver()
        {
            gameOverCallback();
        }

        /// <summary>
        /// Spawn the whole board with columns and with ships inside them
        /// </summary>
        private void SpawnShips()
        {
            RecenterBoard(shipSpawnerInfo.XPadding * shipSpawnerInfo.NumberOfColumn - 1);

            var currentPositionX = Vector3.zero;

            for (int i = 0; i < shipSpawnerInfo.NumberOfColumn; i++)
            {
                var currentPositionY = Vector3.zero;
            
                //Create a new column
                Transform newColumnTransform = Instantiate(shipSpawnerInfo.ColumnContainerPrefab, 
                    Vector3.zero, Quaternion.identity, _transform).transform;
                
                newColumnTransform.name = $"Column {i}";
                
                AlienColumnContainer newColumnComponent =
                    newColumnTransform.GetComponent<AlienColumnContainer>();

                newColumnComponent.Index = i;
                newColumnComponent.EnemiesInColumn = shipSpawnerInfo.NumberInColumn;
                
                //Add this column to the board's list
                _alienColumns.Add(newColumnComponent);

                SpawnShipInsideColumn(newColumnTransform, newColumnComponent, currentPositionY);

                //Set the next column to the right by the X padding
                newColumnTransform.localPosition = currentPositionX;
                currentPositionX.x += shipSpawnerInfo.XPadding;
            }
        }

        /// <summary>
        /// Spawn ships inside columns 
        /// </summary>
        /// <param name="columnTransform"></param>
        /// <param name="columnContainer"></param>
        /// <param name="newYpos"></param>
        private void SpawnShipInsideColumn(Transform columnTransform, 
            AlienColumnContainer columnContainer, Vector3 newYpos)
        {
            for (int j = 0; j < shipSpawnerInfo.NumberInColumn; j++)
            {
                //Create a new ship
                GameObject newShipGameObject = Instantiate(shipSpawnerInfo.AlienShipPrefab,
                    newYpos, Quaternion.identity, columnTransform);
                    
                AlienShip newShipComponent = newShipGameObject.GetComponent<AlienShip>();
                
                //Add the event handler to the parent column
                newShipComponent.DestroyShipCallback += columnContainer.EliminateLowestShipHandler;
                newShipComponent.DestroyShipCallback += uiManager.SetScoreText;
                newShipComponent.GameOverEnterCallback += ForceGameOver;

                //Set the next ship lower by the Y padding
                newYpos.y -= shipSpawnerInfo.YPadding;
            }
        }

        /// <summary>
        /// Recenter the board depending on width of column
        /// </summary>
        /// <param name="width"></param>
        private void RecenterBoard(float width)
        {
            var boardPos = _transform.position;
            boardPos.x -= width / 2.0f;
            _transform.position = boardPos;
        }

        /// <summary>
        /// Move the whole board with a delay
        /// If it touches boundaries, it goes down with delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveBoard()
        {
            yield return new WaitForSeconds(defaultMoveRate);

            while (true)
            {
                _transform.position += Vector3.right * moveDistance;

                yield return new WaitForSeconds(defaultMoveRate);

                if (_alienColumns[0].transform.position.x <= -horizontalBoundDistance ||
                    _alienColumns[_alienColumns.Count - 1].transform.position.x >= horizontalBoundDistance)
                {
                    moveDistance *= -1;
                    _transform.position += Vector3.down * moveDistanceVertical;
                    
                    yield return new WaitForSeconds(defaultMoveRate);
                }
            }
        }
    }
}
