using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts
{
    public class PlayerShip : MonoBehaviour
    {
        #region Variables
        
        [field: SerializeField, 
                Tooltip("Distance from the center for player to stop")] 
        private float horizontalBoundDistance = 3f;

        [field: SerializeField, 
                Tooltip("Speed of the ship")] 
        private float movementSpeed = .005f;

        [field: SerializeField, 
                Tooltip("Point where bullet spawn")] 
        private Transform cannonShip;

        [field: SerializeField, 
                Tooltip("Bullet prefab")] 
        private GameObject bulletPrefab;

        private Transform _transform;

        private bool _lockShooting;
        
        /// <summary>
        /// New input system generated class
        /// </summary>
        private PlayerActions _playerActions;
        
        /// <summary>
        /// Current value of horizontal movement
        /// </summary>
        private float _currentMoveValue;

        private Action _gameOverCallback;

        public Action GameOverCallback
        {
            get => _gameOverCallback;
            set => _gameOverCallback = value;
        }
        
        #endregion

        public void Init()
        {
            _transform = transform;
            EnableControls();
        }

        private void OnDestroy()
        {
            DisableControls();
        }

        #region ControlHandlers

        private void EnableControls()
        {
            _playerActions = new PlayerActions();
            
            _playerActions.PlayerControl.Movement.performed += MoveShipHandler;
            _playerActions.PlayerControl.Movement.canceled += StopShipHandler;
            _playerActions.PlayerControl.Shoot.performed += ShootBulletHandler;
            _playerActions.Enable();
        }

        private void DisableControls()
        {
            _playerActions.Disable();
            _playerActions.PlayerControl.Movement.performed -= MoveShipHandler;
            _playerActions.PlayerControl.Movement.canceled -= StopShipHandler;
            _playerActions.PlayerControl.Shoot.performed -= ShootBulletHandler;
        }

        /// <summary>
        /// Set value for horizontal movement
        /// </summary>
        /// <param name="obj"></param>
        private void MoveShipHandler(InputAction.CallbackContext obj)
        {
            _currentMoveValue = obj.ReadValue<float>();
        }
        
        
        private void StopShipHandler(InputAction.CallbackContext _)
        {
            _currentMoveValue = 0;
        }

        /// <summary>
        /// Spawn a bullet and lock shooting ability until the bullet is destroyed
        /// </summary>
        /// <param name="obj"></param>
        private void ShootBulletHandler(InputAction.CallbackContext obj)
        {
            if (_lockShooting) return;
            
            var bullet = Instantiate(bulletPrefab, cannonShip.position, cannonShip.rotation);
            bullet.GetComponent<BulletPlayer>().AllowToShootEvent = AllowToShootHandler;
            _lockShooting = true;
        }

        #endregion

        /// <summary>
        /// Allow player to shoot again
        /// </summary>
        private void AllowToShootHandler()
        {
            _lockShooting = false;
        }

        /// <summary>
        /// Destroy ship and end the game
        /// </summary>
        public void DestroyShip()
        {
            _gameOverCallback();
            Destroy(gameObject);
        }

        private void Update()
        {
            var position = _transform.position;
            
            float newXVal = Mathf.Clamp(position.x + 
                                           movementSpeed * _currentMoveValue * Time.deltaTime,
                -horizontalBoundDistance, horizontalBoundDistance);
            
            position = new Vector3(newXVal, position.y, position.z);
            _transform.position = position;
        }
    }
}
